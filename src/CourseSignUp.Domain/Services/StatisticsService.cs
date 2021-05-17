using CourseSignUp.Domain.Models;
using CourseSignUp.Domain.Services.Abstractions;
using CourseSignUp.Infraestructure.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseSignUp.Domain.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IStatisticsRepository _statisticsRepo;
        private readonly ICoursesService _coursesService;

        public StatisticsService(IStatisticsRepository repo, ICoursesService coursesService)
        {
            _statisticsRepo = repo;
            _coursesService = coursesService;
        }

        public async Task AggregateStatistics(Course course, ImmutableArray<Student> newStudents, DateTime now)
        {
            if (newStudents.IsEmpty)
                return;
            var maxAge = newStudents.Max(student => CalculateAge(student));

            var minAge = newStudents.Min(student => CalculateAge(student));

            var avgAge = newStudents.Sum(student => CalculateAge(student)) / (decimal)newStudents.Length;

            var statistics = new CourseStatistics(course, maxAge, minAge, avgAge);

            await _statisticsRepo.SaveStatisticsSlice(statistics, newStudents.Length, now);
        }

        private static int CalculateAge(Student student)
        {
            var birthDay = student.DateOfBirth;
            var age = birthDay.Year - DateTime.Now.Year;
            if ((birthDay.Month > DateTime.Now.Month) || (birthDay.Month == DateTime.Now.Month && birthDay.Day > DateTime.Now.Day))
                age--;

            return age;
        }

        public async Task<CourseStatistics> GetStatistics(string courseId, DateTime start, DateTime end)
        {
            var courseTask = _coursesService.FindAsync(courseId);
            var slices = await _statisticsRepo.GetSlices(courseId, start, end);

            var (maxAge, minAge, avgAge, _) = slices.Aggregate(seed(), (curr, next) =>
            {
                var (stats, sliceSize) = next;
                return (Math.Max(curr.maxAge, stats.MaximumAge), Math.Min(curr.minAge, stats.MinimumAge), CalculateNewAverage((curr.avgAge, curr.totalSize), (stats.AverageAge, sliceSize)), curr.totalSize + sliceSize);
            });

            var course = await courseTask;

            return new(
                course,
                maxAge,
                minAge,
                avgAge
                );

            static (int maxAge, int minAge, decimal avgAge, int totalSize) seed() => (int.MinValue, int.MaxValue, 0, 0);
        }

        private static decimal CalculateNewAverage((decimal avgAge, int sliceSize) slice1, (decimal avgAge, int sliceSize) slice2)
            => (slice1.avgAge, slice2.avgAge) switch
            {
                (0, _) => slice2.avgAge,
                (_, 0) => slice1.avgAge,
                _ => (slice1.avgAge * slice1.sliceSize + slice2.avgAge * slice2.sliceSize) / (slice1.sliceSize + slice2.sliceSize)
            };

    }
}
