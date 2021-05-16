using CourseSignUp.Domain.Models;
using System.Threading.Tasks;

namespace CourseSignUp.Domain.Services.Abstractions
{
    public interface ICoursesService
    {
        Task<Course> CreateAsync(string name, Lecturer lecturer, int capacity);
        Task<Course> FindAsync(string id);

        /// <summary>
        /// Atomic check, reduce ammount of available seats from course and add students to course
        /// </summary>
        /// <param name="course"></param>
        /// <returns>True if student was added and false otherwise</returns>
        Task<bool> ConsumeAvailableSpot(Course course, Student student);
    }
}