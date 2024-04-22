using LearningSite.Core.DTOs.Order;
using LearningSite.Core.Services.Interfaces;
using LearningSite.DataLayer.Context;
using LearningSite.DataLayer.Entities.Course;
using LearningSite.DataLayer.Entities.Order;
using LearningSite.DataLayer.Entities.User;
using LearningSite.DataLayer.Entities.Wallet;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LearningSite.Core.Services
{
    public class OrderService : IOrderService
    {
        private LearningSiteContext _context;
        private IUserService _userService;

        public OrderService(LearningSiteContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public void AddDiscount(Discount discount)
        {
            _context.Discounts.Add(discount);
            _context.SaveChanges();
        }

        public int AddOrder(string userName, int courseId)
        {
            int userId = _userService.GetUserIdByUsername(userName);

            Order order = _context.Orders
                .FirstOrDefault(o => o.UserId == userId && !o.IsFinaly);

            var course = _context.Courses.Find(courseId);

            if (order == null)
            {
                order = new Order()
                {
                    UserId = userId,
                    IsFinaly = false,
                    CreateDate = DateTime.Now,
                    OrderSum = course.CoursePrice,
                    OrderDetails = new List<OrderDetail>()
                    {
                        new OrderDetail()
                        {
                            CourseId = courseId,
                            Count = 1,
                            Price = course.CoursePrice
                        }
                    }
                };
                _context.Orders.Add(order);
                _context.SaveChanges();
            }
            else
            {
                OrderDetail detail = _context.OrderDetails
                    .FirstOrDefault(d => d.OrderId == order.OrderId && d.CourseId == courseId);
                if (detail != null)
                {
                    detail.Count += 1;
                    _context.OrderDetails.Update(detail);
                }
                else
                {
                    detail = new OrderDetail()
                    {
                        OrderId = order.OrderId,
                        Count = 1,
                        CourseId = courseId,
                        Price = course.CoursePrice,
                    };
                    _context.OrderDetails.Add(detail);
                }

                _context.SaveChanges();
                UpdatePriceOrder(order.OrderId);
            }


            return order.OrderId;

        }

        public bool FinalyOrder(string username, int OrderId)
        {
            int userid = _userService.GetUserIdByUsername(username);
            var order = _context.Orders.Include(o => o.OrderDetails).ThenInclude(od => od.Course)
                .FirstOrDefault(o => o.OrderId == OrderId && o.UserId == userid);
            if(order==null || order.IsFinaly)
            {
                return false;
            }
            if (_userService.BalanceUserWallet(username) >= order.OrderSum)
            {
                order.IsFinaly = true;
                _userService.AddWallet(new Wallet()
                {
                    Amount = order.OrderSum,
                    CreateDate = DateTime.Now,
                    IsPay = true,
                    Description = "فاکتور شماره #"+order.OrderId,
                    UserId = userid,
                    TypeId = 2
                });
                _context.Orders.Update(order);
                foreach (var detail in order.OrderDetails)
                {
                    _context.UserCourses.Add(new UserCourse()
                    {
                        CourseId = detail.CourseId,
                        UserId = userid
                    });
                }
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public List<Discount> GetAllDiscount()
        {
            return _context.Discounts.ToList();
        }

        public Discount GetDiscountById(int disId)
        {
            return _context.Discounts.Find(disId);
        }

        public Order GetOrderById(int orderId)
        {
            return _context.Orders.Find(orderId);
        }

        public Order GetOrderForUserPanel(string username, int orderId)
        {
            int userid = _userService.GetUserIdByUsername(username);
            return _context.Orders.Include(o => o.OrderDetails).ThenInclude(od=>od.Course)
                .FirstOrDefault(o => o.UserId == userid && o.OrderId == orderId);
        }

        public List<Order> GetUserOrders(string username)
        {
            int userid = _userService.GetUserIdByUsername(username);
            return _context.Orders.Where(o => o.UserId == userid).ToList();
        }

        public bool IsExistCode(string code)
        {
            return _context.Discounts.Any(d => d.DiscountCode == code);
        }

        public bool IsUSerInCourse(string username, int courseId)
        {
            int userId = _userService.GetUserIdByUsername(username);
            return _context.UserCourses.Any(c => c.UserId == userId && c.CourseId == courseId);
        }

        public void UpdateDiscount(Discount discount)
        {
            _context.Discounts.Update(discount);
            _context.SaveChanges();
        }

        public void UpdateOrder(Order order)
        {
            _context.Orders.Update(order);
            _context.SaveChanges();
        }

        public void UpdatePriceOrder(int orderId)
        {
            var order = _context.Orders.Find(orderId);
            order.OrderSum = _context.OrderDetails.Where(d => d.OrderId == orderId).Sum(d => d.Price * d.Count);
            _context.Orders.Update(order);
            _context.SaveChanges();
        }

        public DiscountUseType UseDiscount(int orderid, string code)
        {
            var discount = _context.Discounts.SingleOrDefault(d => d.DiscountCode == code);
            if (discount == null)
            {
                return DiscountUseType.NotFound;
            }
            if (discount.StartDate != null && discount.StartDate < DateTime.Now)
                return DiscountUseType.ExpierDate;
            if(discount.EndDate!=null&&discount.EndDate>=DateTime.Now)
                return DiscountUseType.ExpierDate;

            if (discount.DiscountCode != null && discount.UsableCount < 1)
                return DiscountUseType.Finished;

            var order = GetOrderById(orderid);

            if (_context.UserDiscountCodes.Any(d=>d.UserId==order.UserId&&d.DiscountId==discount.DiscountId))
            {
                return DiscountUseType.UserUsed;
            }

            int percent = (order.OrderSum * discount.DiscountPercent) / 100;
            order.OrderSum = order.OrderSum - percent;
            _context.Orders.Update(order);
            _context.SaveChanges();
            if (discount.UsableCount != null)
            {
                discount.UsableCount -= 1;
            }
            _context.Discounts.Update(discount);
            _context.UserDiscountCodes.Add(new UserDiscountCode()
            {
                UserId = order.UserId,
                DiscountId = discount.DiscountId
            });
            _context.SaveChanges();
            

            return DiscountUseType.Success;
        }
    }
}
