using CourseSignUp.Domain.Services;
using CourseSignUp.Domain.Services.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseSignUp.Domain.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCourseSignUpDomain(this IServiceCollection services)
        {
            services.AddScoped<ICoursesService, CoursesService>();
            services.AddScoped<IStatisticsService, StatisticsService>();
            return services;
        }
    }
}
