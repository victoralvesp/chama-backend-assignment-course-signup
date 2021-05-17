using CourseSignUp.Domain.Models;

namespace CourseSignUp.Infraestructure.Messages
{
    public record SignUpProcessedMessage(string CourseId, Student Student, bool StudentAccepted);
}
