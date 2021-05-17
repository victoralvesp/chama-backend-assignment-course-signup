using CourseSignUp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseSignUp.Domain.Services.Abstractions
{
    public interface IStatisticsService
    {
        Task AggregateStatistics(Course course, ImmutableArray<Student> newStudents, DateTime now);
        Task<CourseStatistics> GetStatistics(string courseId, DateTime start, DateTime end);
    }
}
