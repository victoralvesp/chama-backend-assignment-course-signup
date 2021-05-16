using System.Collections.Immutable;

namespace CourseSignUp.Domain.Models
{
    public record Course(string Id, Lecturer Lecturer, string Name, int Capacity)
    {
        public int NumberOfStudents => Students.Length;
        public int SeatsAvailable { get; init; } = Capacity;
        public ImmutableArray<Student> Students { get; init; }
    }
}