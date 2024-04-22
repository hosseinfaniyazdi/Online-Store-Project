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
    [PermissionChecker(4)]
    public class EditUserModel : PageModel
    {
        private IUserService _userService;
        private IPermissionService _permissionService;

        public EditUserModel(IUserService userService, IPermissionService permissionService)
        {
            _userService = userService;
            _permissionService = permissionService;
        }

        [BindProperty]
        public EditUserViewModel EditUserViewModel { get; set; }
        public void OnGet(int id)
        {
            EditUserViewModel = _userService.GetUserForShowEditMode(id);
            ViewData["Roles"] = _permissionService.GetRoles();
        }
        public IActionResult OnPost(List<int> SelectedRoles)
        {
            if (!ModelState.IsValid)
            {
                EditUserViewModel = _userService.GetUserForShowEditMode(EditUserViewModel.UserId);
                ViewData["Roles"] = _permissionService.GetRoles();
                return Page();
                //return Redirect("/Admin/Users/EditUser/"+EditUserViewModel.UserId);
            }
            _userService.EditUserFromAdmin(EditUserViewModel);
            _permissionService.EditRolesUser(EditUserViewModel.UserId, SelectedRoles);
            return RedirectToPage("Index");
        }
    }
}