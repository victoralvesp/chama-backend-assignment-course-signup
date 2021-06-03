using System.Threading.Tasks;
using CourseSignUp.Domain.Models;

namespace CourseSignUp.Infraestructure.Abstractions
{
    public interface IPaymentInfoRepository
    {
        Task<double> GetCourseSubscriptionCost(string courseId);
        Task<CreditInfo> GetCreditInfo(string email);
    }

    

}