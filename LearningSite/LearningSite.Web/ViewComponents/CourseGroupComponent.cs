﻿using LearningSite.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearningSite.Web.ViewComponents
{
    public class CourseGroupComponent : ViewComponent
    {
        private ICourseService _courseService;

        public CourseGroupComponent(ICourseService courseService)
        {
            _courseService = courseService;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.FromResult((IViewComponentResult)View("CourseGroup", _courseService.GetAllGroup()));
        }
    }
}
