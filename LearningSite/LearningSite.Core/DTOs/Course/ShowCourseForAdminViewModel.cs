using System;
using System.Collections.Generic;
using System.Text;
using LearningSite.DataLayer.Entities.Course;

namespace LearningSite.Core.DTOs.Course
{
    public class ShowCourseForAdminViewModel
    {
        public int CourseId { get; set; }
        public string Title { get; set; }
        public string ImageName { get; set; }
        public int EpisodeCount { get; set; }
        //public int CurrentPage { get; set; }
        //public int PageCount { get; set; }

    }
}
