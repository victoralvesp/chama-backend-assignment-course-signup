using System;
using System.ComponentModel.DataAnnotations;

namespace CourseSignUp.Api.Courses
{
    public class SignUpToCourseDto
    {

        [Required]
        public string CourseId { get; set; }

        [Required]
        public StudentDto Student { get; set; }

        public class StudentDto
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
            [Required]
            public string Name { get; set; }
            
            [Required]
            [DataType(DataType.Date)]
            public DateTime DateOfBirth { get; set; }
        }
    }
}