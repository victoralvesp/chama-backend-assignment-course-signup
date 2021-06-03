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
        private readonly IWaitListRepository _waitListRepo;

        public CoursesService(ICoursesRepository coursesRepo, IWaitListRepository waitListRepo)
        {
            _coursesRepo = coursesRepo;
            _waitListRepo = waitListRepo;
        }

        public async Task<int> ConsumeSeatAvailable(Course course, Student student)
        {
            if (course.SeatsAvailable > 0 && await _coursesRepo.ConsumeSeatAvailable(course, student))
            {
                return 0;
            }
            else
            {
                return await _waitListRepo.Add(course, student);
            }
        }

        public async Task<Course> CreateAsync(string name, Lecturer lecturer, int capacity)
        {
            var course = new Course(string.Empty, lecturer, name, capacity);

            course = await _coursesRepo.WriteNewAsync(course);

            return course;
        }

        public async Task<Course> FindAsync(string id)
        {
            var course = await _coursesRepo.FindAsync(id);

            return course;
        }

        public Task<Student> GetNextInWaitList(string courseId)
        {
            throw new NotImplementedException();
        }

        public Task ReleaseSeat(Course course, Student student)
        {
            throw new NotImplementedException();
        }
    }
}
