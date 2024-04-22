using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearningSite.Core.Services.Interfaces;
using LearningSite.DataLayer.Entities.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LearningSite.Core.Services.Interfaces;
using LearningSite.DataLayer.Entities.User;
using LearningSite.Core.Security;

namespace LearningSite.Web.Pages.Admin.Roles
{
    [PermissionChecker(8)]
    public class CreateRoleModel : PageModel
    {
        private IPermissionService _permissionService;

        public CreateRoleModel(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        
        [BindProperty]
        public Role Role { get; set; }

        public void OnGet()
        {
            ViewData["Permission"] = _permissionService.GetAllPermission();
        }

        public IActionResult OnPost(List<int> SelectedPermission)
        {
            if (!ModelState.IsValid)
                return Page();

           
            Role.IsDelete = false;
            int roleId = _permissionService.AddRole(Role);
            _permissionService.AddPermissionsToRoles(roleId, SelectedPermission);

            //TODO Add Permission

            return RedirectToPage("Index");
        }
    }
}