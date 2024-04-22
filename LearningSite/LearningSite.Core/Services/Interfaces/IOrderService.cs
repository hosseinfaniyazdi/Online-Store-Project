using LearningSite.Core.DTOs.Order;
using LearningSite.DataLayer.Entities.Order;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearningSite.Core.Services.Interfaces
{
    public interface IOrderService
    {
        int AddOrder(string userName, int courseId);

        void UpdatePriceOrder(int orderId);
        Order GetOrderForUserPanel(string username, int orderId);
        Order GetOrderById(int orderId);

        bool FinalyOrder(string username ,int OrderId);

        List<Order> GetUserOrders(string username);
        void UpdateOrder(Order order);
        bool IsUSerInCourse(string username, int courseId);

        #region Discount
        DiscountUseType UseDiscount(int orderid, string code);
        void AddDiscount(Discount discount);
        List<Discount> GetAllDiscount();
        Discount GetDiscountById(int disId);
        void UpdateDiscount(Discount discount);
        bool IsExistCode(string code);
        #endregion
    }

}
