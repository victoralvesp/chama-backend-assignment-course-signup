using CourseSignUp.Domain.Models;
using CourseSignUp.Domain.Services.Abstractions;
using CourseSignUp.Infraestructure.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseSignUp.Domain.Services
{
    public class CoursesService : ICoursesService
    {
        private readonly ICoursesRepository _coursesRepo;

        public CoursesService(ICoursesRepository coursesRepo)
        {
            _coursesRepo = coursesRepo;
        }

        public async Task<Course> CreateAsync(string name, Lecturer lecturer, int capacity)
        {
            var course = new Course(string.Empty, lecturer, name, capacity, ImmutableArray.Create<Student>());

            course = await _coursesRepo.WriteNewAsync(course);

            return course;
        }

        public async Task<Course> FindAsync(string id)
        {
            var course = await _coursesRepo.FindAsync(id);

            return course;
        }
    }
}
