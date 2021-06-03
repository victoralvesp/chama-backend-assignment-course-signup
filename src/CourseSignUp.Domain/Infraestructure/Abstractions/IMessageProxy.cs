using System;
using System.Threading.Tasks;

namespace CourseSignUp.Infraestructure.Abstractions
{
    public interface IMessageProxy
    {
        Task SendToTopic<T>(string topic, T message);
        IDisposable SubscribeToTopic<T>(string topicName, Action<T> onSignUpMessageReceived);
    }

}