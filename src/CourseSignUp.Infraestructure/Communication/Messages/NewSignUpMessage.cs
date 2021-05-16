using CourseSignUp.Domain.Models;

namespace CourseSignUp.Infraestructure.Messages
{
    public record NewSignUpMessage(string CourseId, Student Student);

    public record SignUpProcessedMessage(string CourseId, Student Student, bool StudentAccepted);
}
