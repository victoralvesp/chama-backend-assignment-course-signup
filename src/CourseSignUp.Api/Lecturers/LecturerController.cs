using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CourseSignUp.Api.Lecturers
{
    [ApiController, Route("api/v1/[controller]")]
    public class LecturersController : ControllerBase
    {
        [HttpPost]
        public Task<IActionResult> Post([FromBody]CreateLecturerDto createStudentDto)
        {
            throw new NotImplementedException();
        }
    }
}