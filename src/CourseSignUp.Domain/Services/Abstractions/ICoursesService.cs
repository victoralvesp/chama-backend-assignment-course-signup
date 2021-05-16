using CourseSignUp.Domain.Models;

namespace CourseSignUp.Domain.Services.Abstractions
{
    public interface ICoursesService
    {
        Course Create(string name, Lecturer lecturer, int capacity) ;
    }
}