using System.Threading.Tasks;

namespace CourseSignUp.Infraestructure.Abstractions
{
    public interface IMessageBusService
    {
        Task SendToTopic(string topic, string message);
    }
}