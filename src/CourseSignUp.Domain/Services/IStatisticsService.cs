using CourseSignUp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseSignUp.Domain.Services
{
    public interface IStatisticsService
    {
        void AggregateStatistics(Task<Course> course, IGrouping<string, Student> newStudents, DateTime now);
    }
}
