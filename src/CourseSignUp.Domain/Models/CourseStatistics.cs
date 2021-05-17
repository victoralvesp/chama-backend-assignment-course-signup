namespace CourseSignUp.Domain.Models
{
    public record CourseStatistics(Course Course, int MaximumAge, int MinimumAge, decimal AverageAge);
    
}