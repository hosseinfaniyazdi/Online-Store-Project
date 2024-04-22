using LearningSite.Core.DTOs.Course;
using LearningSite.DataLayer.Entities.Course;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearningSite.Core.Services.Interfaces
{
    public interface ICourseService
    {
        #region Group

        List<CourseGroup> GetAllGroup();
        List<SelectListItem> GetGroupForManageCourse();
        List<SelectListItem> GetSubGroupForManageCourse(int groupId);
        List<SelectListItem> GetTeachers();
        List<SelectListItem> GetLevels();
        List<SelectListItem> GetStatues();


        #endregion

        #region Course
        Tuple<List<ShowCourseForAdminViewModel>, int> GetCoursesForAdmin(int pageId = 1, string FilterTitle = "");
        int AddCourse(Course course, IFormFile imgCourse, IFormFile courseDemo);
        Course GetCourseById(int courseId);
        void UpdateCourse(Course course, IFormFile imgCourse, IFormFile courseDemo);
        Tuple<List<ShowCourseListItemViewModel>,int> GetCourse(int pageId = 1, string filter = "", string getType = "all", string orderByType = "date", int startPrice = 0, int endPrice = 0, List<int> SelectedGroups = null,int take=0);
        Course GetCourseForShow(int courseId);
        List<ShowCourseListItemViewModel> GetPopularCours();
        #endregion

        #region Episode
        List<CourseEpisode> GetListEpisodeCourse(int courseId);
        bool CheckExistFile(string fileName);
        int AddEpisode(CourseEpisode episode, IFormFile episodeFile);
        CourseEpisode GetEpisodeById(int courseId);
        void EditEpisode(CourseEpisode episode, IFormFile episodeFile);
        #endregion

        #region Comment
        void AddComment(CourseComment comment);
        Tuple<List<CourseComment>,int> GetCourseComment(int courseId,int pageId=1);
        #endregion
    }
}
