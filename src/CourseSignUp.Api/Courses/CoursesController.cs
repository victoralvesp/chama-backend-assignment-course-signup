using CourseSignUp.Domain.Services.Abstractions;
using CourseSignUp.Infraestructure.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace CourseSignUp.Api.Courses
{
    [ApiController, Route("[controller]")]
    public class CoursesController : ControllerBase
    {
        ICoursesService _coursesService;
        ILecturerRepository _lecturerRepo;
        ICoursesRepository _coursesRepo;
        private IMessageBusService _messageBusService;

        [HttpGet, Route("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var course = _coursesRepo.Find(id);
            return Ok(new CourseDto(course));
        }

        [HttpPost, Route("")]
        public async Task<IActionResult> Post([FromBody] CreateCourseDto createCourseDto)
        {
            var lecturerId = createCourseDto.LecturerId;
            var lecturer = await _lecturerRepo.FindAsync(lecturerId);
            var course = _coursesService.Create(createCourseDto.Name, lecturer, createCourseDto.Capacity);
            course = await _coursesRepo.WriteNewAsync(course);

            return Ok(course);
        }

        [HttpPost, Route("{id}/sign-up")]
        public async Task<IActionResult> Post([FromBody] SignUpToCourseDto signUpToCourseDto)
        {
            await _messageBusService.SendToTopic(Constants.NEW_SIGN_UP, JsonSerializer.Serialize(signUpToCourseDto));
            return Ok(SystemTexts.NotifyUserThatTheyWillBeInformedByEmail(signUpToCourseDto.Student.Name));
        }
    }
}
