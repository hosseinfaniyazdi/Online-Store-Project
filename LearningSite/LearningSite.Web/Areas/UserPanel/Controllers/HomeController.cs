using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearningSite.Core.DTOs;
using LearningSite.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearningSite.Web.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    [Authorize]
    public class HomeController : Controller
    {
        private IUserService _userService;
        public HomeController(IUserService userService)
        {
            _userService = userService;
        }
        public IActionResult Index()
        {
            return View(_userService.GetUserInformation(User.Identity.Name));
        }

        [Route("UserPanel/EditProfile")]
        public IActionResult EditProfile()
        {
            
            return View(_userService.GetDataForEditProfileUser(User.Identity.Name));
        }

        [HttpPost]
        [Route("UserPanel/EditProfile")]
        public IActionResult EditProfile(EditProfileViewModel edit)
        {

            if (!ModelState.IsValid)
                return View(edit);
            _userService.EditProfile(User.Identity.Name, edit);
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/Login?EditProfile=true");
        }

        [Route("UserPanel/ChangePassword")]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [Route("UserPanel/ChangePassword")]
        public IActionResult ChangePassword(ChangePasswordViewModel change)
        {
            string CurrenUsername = User.Identity.Name;
            if (!ModelState.IsValid)
                return View(change);
            if (!_userService.CompareOldPassword(change.OldPassword, CurrenUsername))
            {
                ModelState.AddModelError("OldPassword", "کلمه عبور فعلی صحیح نمی باشد");
                return View(change);
            }
            _userService.ChangeUserPassword(CurrenUsername, change.Password);
            ViewBag.IsSuccess = true;
            return View();
        }


    }
}