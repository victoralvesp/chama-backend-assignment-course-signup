using System.Threading.Tasks;

namespace CourseSignUp.Infraestructure.Abstractions
{
    public interface IMessageBusService
    {
        Task SendToTopic<T>(string topic, T message);

    }
}