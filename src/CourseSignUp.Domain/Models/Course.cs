using System.Collections.Immutable;

namespace CourseSignUp.Domain.Models
{
    public record Course
    {
        public Course(string id, Lecturer lecturer, string name, int capacity, int seatsAvailable = -1, ImmutableArray<Student> students = default)
        {
            Capacity = capacity;
            Id = id;
            Lecturer = lecturer;
            Name = name;
            SeatsAvailable = seatsAvailable < 0 ? capacity : seatsAvailable;
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