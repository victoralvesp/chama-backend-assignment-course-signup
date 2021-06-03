using CourseSignUp.Domain.Models;

namespace CourseSignUp.Infraestructure.Messages
{
    public record SignUpProcessedMessage(string CourseId, Student Student, SignUpStatus Status)
    {
        public bool StudentAccepted => Status == SignUpStatus.Accepted;
    }

    public enum SignUpStatus
    {
        New,
        SeatReserved,
        Accepted,
        OnWaitList,
        CreditRefused
    }
}
