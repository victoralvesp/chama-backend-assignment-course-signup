using CourseSignUp.Domain.Models;

namespace CourseSignUp.Infraestructure.Messages
{
    public record NewSignUpMessage(string CourseId, Student Student);
}
