using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearningSite.Core.DTOs;
using LearningSite.Core.Security;
using LearningSite.Core.Services.Interfaces;
using LearningSite.DataLayer.Entities.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LearningSite.Web.Pages.Admin.Roles
{
    [PermissionChecker(7)]
    public class IndexModel : PageModel
    {
        private IPermissionService _permissionService;

        public IndexModel(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        public List<Role> RolesList { get; set; }


        public void OnGet()
        {
            RolesList = _permissionService.GetRoles();
        }


    }
}