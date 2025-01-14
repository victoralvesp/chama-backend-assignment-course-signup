﻿using CourseSignUp.Domain.Services;
using CourseSignUp.Domain.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CourseSignUp.Api.Statistics
{
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsService _statisticsService;
        private readonly ICoursesService _coursesService;

        public StatisticsController(IStatisticsService statisticsService, ICoursesService coursesService)
        {
            _statisticsService = statisticsService;
            _coursesService = coursesService;
        }

        [HttpGet, Route("/api/v1/courses/{courseId}/statistics")]
        public async Task<IActionResult> Get(string courseId, [FromQuery] DateTime? start, [FromQuery]DateTime? end)
        {
            start ??= DateTime.MinValue;
            end ??= DateTime.MaxValue;
            var statistics = await _statisticsService.GetStatistics(courseId, start.Value, end.Value);
            return Ok(new CourseStatisticsDto(statistics));
        }
    }
}