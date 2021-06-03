using CourseSignUp.Domain;
using CourseSignUp.Infraestructure.Messages;
using CourseSignUp.Domain.Services.Abstractions;
using CourseSignUp.Infraestructure.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace CourseSignUp.Api.Courses
{
    [ApiController, Route("api/v1/[controller]")]
    public class CoursesController : ControllerBase
    {
        private readonly ICoursesService _coursesService;
        private readonly ILecturerRepository _lecturerRepo;
        private readonly IMessageProxy _messageBusService;

        [HttpGet, Route("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var course = await _coursesService.FindAsync(id);
            return Ok(new CourseDto(course));
        }

        [HttpPost, Route("")]
        public async Task<IActionResult> Post([FromBody] CreateCourseDto createCourseDto)
        {
            var lecturerId = createCourseDto.LecturerId;
            var lecturer = await _lecturerRepo.FindAsync(lecturerId);
            var course = await _coursesService.CreateAsync(createCourseDto.Name, lecturer, createCourseDto.Capacity);
            

            return Ok(course);
        }

        [HttpPost, Route("{id}/sign-up")]
        public async Task<IActionResult> Post([FromBody] SignUpToCourseDto signUpToCourseDto)
        {
            await _messageBusService.SendToTopic(Constants.NEW_SIGN_UP_TOPIC, new NewSignUpMessage(signUpToCourseDto.CourseId, signUpToCourseDto.Student.ToDomain()));
            return Ok(ChamaSystemTexts.NotifyUserThatTheyWillBeInformedByEmail(signUpToCourseDto.Student.Name));
        }
    }
}
