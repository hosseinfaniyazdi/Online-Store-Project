using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearningSite.Core.DTOs;
using LearningSite.Core.Security;
using LearningSite.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LearningSite.Web.Pages.Admin.Users
{
    [PermissionChecker(5)]
    public class DeleteUserModel : PageModel
    {
        private IUserService _userService;

        public DeleteUserModel(IUserService userService)
        {
            _userService = userService;
        }
        public InformationUserViewModel InformationUserViewModel { get; set; }
        public void OnGet(int id)
        {
            ViewData["UserId"] = id;
            InformationUserViewModel = _userService.GetUserInformation(id);
        }

        public IActionResult OnPost(int UserId)
        {
            _userService.DeleteUser(UserId);
            return RedirectToPage("Index");
        }
    }
}