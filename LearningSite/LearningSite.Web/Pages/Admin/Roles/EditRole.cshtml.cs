using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearningSite.Core.Security;
using LearningSite.Core.Services.Interfaces;
using LearningSite.DataLayer.Entities.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LearningSite.Web.Pages.Admin.Roles
{
    [PermissionChecker(9)]
    public class EditRoleModel : PageModel
    {
        private IPermissionService _permissionService;

        public EditRoleModel(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        [BindProperty]
        public Role Role { get; set; }
        public void OnGet(int id)
        {
            Role = _permissionService.GetRoleById(id);
            ViewData["Permission"] = _permissionService.GetAllPermission();
            ViewData["SelectedPermissions"] = _permissionService.permissionsrole(id);
        }

        public IActionResult OnPost(List<int> SelectedPermission)
        {
            if (!ModelState.IsValid)
                return Page();


            _permissionService.EditRole(Role);

            _permissionService.UpdatePermissionsRole(Role.RoleId, SelectedPermission);

            return RedirectToPage("Index");
        }
    }
}