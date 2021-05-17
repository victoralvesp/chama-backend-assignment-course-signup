using CourseSignUp.Domain.Models;
using System;
using System.Collections.Immutable;
using System.Threading.Tasks;

namespace CourseSignUp.Infraestructure.Abstractions
{
    public interface IStatisticsRepository
    {
        Task SaveStatisticsSlice(CourseStatistics statistics, int sliceSize, DateTime now);
        Task<ImmutableArray<(CourseStatistics stats, int sliceSize)>> GetSlices(string courseId, DateTime start, DateTime end);
    }
}