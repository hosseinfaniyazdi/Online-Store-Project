using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LearningSite.Core.Services.Interfaces;
using LearningSite.DataLayer.Entities.Course;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearningSite.Web.Controllers
{
    public class CourseController : Controller
    {
        private ICourseService _courseService;
        private IOrderService _orderService;
        private IUserService _userService;

        public CourseController(ICourseService courseService, IOrderService orderService, IUserService userService)
        {
            _courseService = courseService;
            _orderService = orderService;
            _userService = userService;
        }
        public IActionResult Index(int pageId = 1, string filter = "", string getType = "all", string orderByType = "date", int startPrice = 0, int endPrice = 0, List<int> SelectedGroups = null)
        {
            ViewBag.selectedGroups = SelectedGroups;
            ViewBag.Groups = _courseService.GetAllGroup();
            ViewBag.pageId = pageId;
            return View(_courseService.GetCourse(pageId,filter,getType,orderByType,startPrice,endPrice,SelectedGroups,9));
        }

        [Route("ShowCourse/{id}")]
        public IActionResult ShowCourse(int id)
        {
            var course = _courseService.GetCourseForShow(id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        [Authorize]
        public IActionResult BuyCourse(int id)
        {
           int orderId = _orderService.AddOrder(User.Identity.Name, id);
            return Redirect("/UserPanel/MyOrders/ShowOrder/" + orderId);
        }

        [Route("DownloadFile/{episodeId}")]
        public IActionResult DownloadFile(int episodeId)
        {
            var episode = _courseService.GetEpisodeById(episodeId);
            string filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/courseFiles", episode.EpisodeFileName);
            string filename = episode.EpisodeFileName;
            if (episode.IsFree)
            {
                byte[] file = System.IO.File.ReadAllBytes(filepath);
                return File(file, "application/force-download", filename);
            }
            if (User.Identity.IsAuthenticated)
            {
                if (_orderService.IsUSerInCourse(User.Identity.Name, episode.CourseId))
                {
                    byte[] file = System.IO.File.ReadAllBytes(filepath);
                    return File(file, "application/force-download", filename);
                }
            }
            return Forbid();
        }



        [HttpPost]
        public IActionResult CreateComment(CourseComment comment)
        {
            comment.IsDelete = false;
            comment.CreateDate = DateTime.Now;
            comment.UserId = _userService.GetUserIdByUsername(User.Identity.Name);
            _courseService.AddComment(comment);
            return View("ShowComment",_courseService.GetCourseComment(comment.CourseId));
        }

        public IActionResult ShowComment(int id,int pageId=1)
        {
            return View(_courseService.GetCourseComment(id, pageId));
        }
    }
}