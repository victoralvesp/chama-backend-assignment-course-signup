using System;
using System.Threading;
using System.Threading.Tasks;
using CourseSignUp.Domain;
using CourseSignUp.Infraestructure.Abstractions;
using CourseSignUp.Infraestructure.Messages;
using Microsoft.Extensions.Hosting;

namespace CourseSignUp.PaymentService.Communication
{
    /// <inheritdoc [cref="IHostedService"]/>
    public class MessageListener : IHostedService
    {
        IMessageProxy _messageProxy;
        IPaymentInfoRepository _paymentInfoRepo;

        public MessageListener(IPaymentInfoRepository paymentInfoService, IMessageProxy messageProxy)
        {
            _paymentInfoRepo = paymentInfoService;
            _messageProxy = messageProxy;
        }

        private CancellationTokenSource _cts;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _cts = new();
            cancellationToken.Register(() => _cts.Cancel());
            var internalToken = _cts.Token;
            StartListen(internalToken);

            return Task.CompletedTask;
        }

        private void StartListen(CancellationToken internalToken)
        {
            _messageProxy.SubscribeToTopic<SignUpProcessedMessage>(Constants.SIGN_UP_PROCESSED_TOPIC, OnSignUpMessageReceived);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public async void OnSignUpMessageReceived(SignUpProcessedMessage message)
        {
            var (courseId, student, _) = message;

            var cost = await _paymentInfoRepo.GetCourseSubscriptionCost(courseId);
            var creditInfo = await _paymentInfoRepo.GetCreditInfo(student.Email);

            if (message.StudentAccepted)
            {
                _ = creditInfo.ConsumeCredit(cost, out var accepted);
                if (accepted)
                {
                    await _messageProxy.SendToTopic(Constants.SIGN_UP_FINISHED, message with { Status = SignUpStatus.Accepted });
                }
                else
                {
                    var refusedMessage = message with { Status = SignUpStatus.CreditRefused };
                    await _messageProxy.SendToTopic(Constants.ROLL_BACK_SEAT_RESERVATION, refusedMessage);
                }
            }
        }
    }
}