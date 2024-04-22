using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearningSite.Core.DTOs.Order;
using LearningSite.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearningSite.Web.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    [Authorize]
    public class MyOrdersController : Controller
    {
        private IOrderService _orderService;
        public MyOrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        public IActionResult Index()
        {
            return View(_orderService.GetUserOrders(User.Identity.Name));
        }
        public IActionResult ShowOrder(int id,bool finaly=false,string type="")
        {
            var order = _orderService.GetOrderForUserPanel(User.Identity.Name, id);
            if (order == null)
            {
                return NotFound();
            }
            ViewBag.typeDiscount = type;
            ViewBag.finaly = finaly;
            return View(order);
        }

        public IActionResult FinalyOrder(int Id)
        {
            if (_orderService.FinalyOrder(User.Identity.Name,Id))
            {
                return Redirect("/UserPanel/MyOrders/ShowOrder/" + Id + "?finaly=true");
            }
            return BadRequest();
        }
        public IActionResult UseDiscount(int orderid,string code)
        {
            DiscountUseType type = _orderService.UseDiscount(orderid, code);
            return Redirect("/UserPanel/MyOrders/ShowOrder/" + orderid + "?type=" + type.ToString());
        }
    }
}