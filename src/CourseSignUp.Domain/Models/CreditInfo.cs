namespace CourseSignUp.Domain.Models
{
    public record CreditInfo(string StudentEmail, double Credit)
    {
        public CreditInfo ConsumeCredit(double value, out bool accepted)
        {
            if (Credit >= value)
            {
                accepted = true;
                return this with { Credit = Credit - value };
            }
            accepted = false;
            return this;
        }
    }
}