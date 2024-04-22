using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearningSite.Core.DTOs.Course;
using LearningSite.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LearningSite.Web.Pages.Admin.Courses
{
    public class IndexModel : PageModel
    {
        private ICourseService _courseService;

        public IndexModel(ICourseService courseService)
        {
            _courseService = courseService;
        }
        public Tuple<List<ShowCourseForAdminViewModel>, int> ShowCourseForAdminViewModels { get; set; }
        public void OnGet(int pageId = 1, string FilterTitle = "")
        {
            ShowCourseForAdminViewModels = _courseService.GetCoursesForAdmin(pageId,FilterTitle);

        }
    }
}