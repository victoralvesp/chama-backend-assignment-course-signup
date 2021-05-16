using System.Collections.Immutable;

namespace CourseSignUp.Domain.Models
{
    public record Course
    {
        public Course(string id, Lecturer lecturer, string name, int capacity, ImmutableArray<Student> students)
        {
            Capacity = capacity;
            Id = id;
            Lecturer = lecturer;
            Name = name;
            SeatsAvailable = capacity;
            Students = students;
        }

        public int Capacity { get; }
        public int NumberOfStudents => Students.Length;
        public string Id { get; }
        public Lecturer Lecturer { get; }
        public string Name { get; }
        public int SeatsAvailable { get; }
        public ImmutableArray<Student> Students { get; }
    }
}