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
        /// <returns>0 if student was added and the order of the student on the wait list otherwise</returns>
        Task<int> ConsumeSeatAvailable(Course course, Student student);
        Task ReleaseSeat(Course course, Student student);

        /// <summary>
        /// Atomic check and get next student in wait list
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        Task<Student> GetNextInWaitList(string courseId);
    }
}