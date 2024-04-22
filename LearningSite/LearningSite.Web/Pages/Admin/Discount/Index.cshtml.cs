 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearningSite.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LearningSite.Web.Pages.Admin.Discount
{
    public class IndexModel : PageModel
    {
        private IOrderService _OrderService;
        public IndexModel(IOrderService orderService)
        {
            _OrderService = orderService;
        }
        public List<DataLayer.Entities.Order.Discount> discounts { get; set; }
        public void OnGet()
        {
            discounts = _OrderService.GetAllDiscount();
        }
    }
}