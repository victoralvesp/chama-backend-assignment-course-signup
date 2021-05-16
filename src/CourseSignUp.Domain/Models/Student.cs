using System;

namespace CourseSignUp.Domain.Models
{
    public record Student(string Name, DateTime DateOfBirth, string Email);
}