using CourseSignUp.Domain.Models;
using System.Threading.Tasks;

namespace CourseSignUp.Infraestructure.Abstractions
{
    public interface ILecturerRepository
    {
        Task<Lecturer> FindAsync(string lecturerId);
    }
}