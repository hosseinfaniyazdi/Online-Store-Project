using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearningSite.Core.DTOs;
using LearningSite.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LearningSite.Web.Pages.Admin.Users
{
    public class ListDeleteUsersModel : PageModel
    {
        private IUserService _userService;
        public ListDeleteUsersModel(IUserService userService)
        {
            _userService = userService;
        }

        public UserForAdminViewModel UserForAdminViewModel { get; set; }
        public void OnGet(int pageId = 1, string Filterusername = "", string Filteremail = "")
        {

            UserForAdminViewModel = _userService.GetDeleteUsers(pageId, Filteremail, Filterusername);
        }
    }
}