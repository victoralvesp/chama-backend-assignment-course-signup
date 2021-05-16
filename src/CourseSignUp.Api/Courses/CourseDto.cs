using CourseSignUp.Domain.Models;

namespace CourseSignUp.Api.Courses
{
    public class CourseDto
    {
        private Course course;

        //TODO add automapper
        public CourseDto(Course course)
        {
            Id = course.Id;
            Capacity = course.Capacity;
            NumberOfStudents = course.NumberOfStudents;
        }

        public string Id { get; set; }
        public int Capacity { get; set; }
        public int NumberOfStudents { get; set; }
    }
}