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
    public class RestoreUserModel : PageModel
    {
        private IUserService _userService;

        public RestoreUserModel(IUserService userService)
        {
            _userService = userService;
        }
        //public InformationUserViewModel InformationUserViewModel { get; set; }
        public void OnGet(int id)
        {
            ViewData["UserId"] = id;
            //InformationUserViewModel = _userService.GetUserInformation(id);
        }
        public IActionResult OnPost(int UserId)
        {
            _userService.RestoreUser(UserId);
            return RedirectToPage("Index");
        }
    }
}