using CourseSignUp.Domain.Models;
using System.Threading.Tasks;

namespace CourseSignUp.Infraestructure.Abstractions
{
    public interface ICoursesRepository
    {
        Task<Course> WriteNewAsync(Course course);
    }
}