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
    [PermissionChecker(2)]
    public class IndexModel : PageModel
    {
        private IUserService _userService;
        public IndexModel(IUserService userService)
        {
            _userService = userService;
        }

        public UserForAdminViewModel userForAdminViewModel { get; set; }
        public void OnGet(int pageId=1,string Filterusername = "" , string Filteremail = "")
        {

            userForAdminViewModel = _userService.GetUsers(pageId, Filteremail, Filterusername);
        }

        
    }
}