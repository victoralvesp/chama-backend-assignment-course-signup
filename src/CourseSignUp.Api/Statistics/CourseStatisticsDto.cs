using CourseSignUp.Domain.Models;

namespace CourseSignUp.Api.Statistics
{
    public class CourseStatisticsDto
    {
        public CourseStatisticsDto()
        {
        }

        public CourseStatisticsDto(CourseStatistics statistics)
        {
            CourseId = statistics.Course.Id;
            CourseName = statistics.Course.Name;
            MaximumAge = statistics.MaximumAge;
            MinimumAge = statistics.MinimumAge;
            AverageAge = statistics.AverageAge;
        }

        public string CourseId { get; set; }
        public string CourseName { get; set; }
        public int MinimumAge { get; set; }
        public int MaximumAge { get; set; }
        public decimal AverageAge { get; set; }
    }
}