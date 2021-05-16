using CourseSignUp.Domain.Models;
using System.Threading.Tasks;

namespace CourseSignUp.Infraestructure.Abstractions
{
    public interface ICoursesRepository
    {
        Task<Course> WriteNewAsync(Course course);
        Task<Course> FindAsync(string id);

        /// <summary>
        /// Atomic decrease available seats and add student to students list
        /// </summary>
        /// <param name="course"></param>
        /// <param name="student"></param>
        /// <returns></returns>
        Task<bool> ConsumeSeatAvailable(Course course, Student student);
    }
}