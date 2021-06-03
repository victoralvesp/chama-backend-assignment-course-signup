using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using CourseSignUp.Domain;
using CourseSignUp.Domain.Services.Abstractions;
using CourseSignUp.Infraestructure.Abstractions;
using CourseSignUp.Infraestructure.Messages;

namespace CourseSignUp.SignupProcessor
{

    /// <summary>
    /// ProcessSignUp starts the course signing Saga and rolls back seat reservetion in case its needed
    /// </summary>
    /// <param name="message"></param>
    /// <param name="log"></param>
    /// <returns></returns>
    public class ProcessSignUp
    {
        private readonly ICoursesService _coursesServiceProxy;

        private readonly IMessageProxy _messageProxy;

        public ProcessSignUp(ICoursesService coursesServiceProxy, IMessageProxy messageProxy)
        {
            _coursesServiceProxy = coursesServiceProxy;
            _messageProxy = messageProxy;
        }



        [FunctionName("ProcessSignUp")]
        public async System.Threading.Tasks.Task ProcessAsync([ServiceBusTrigger(Constants.NEW_SIGN_UP_TOPIC, Constants.ALL_MESSAGES_SUBSCRIPTION, Connection = Constants.SERVICE_BUS_CONNECTION_NAME)] NewSignUpMessage message, ILogger log)
        {
            log.LogInformation($"[INF-0001] New sign up received: {message}");

            var (courseId, student) = message;
            var course = await _coursesServiceProxy.FindAsync(courseId);


            if (!course.Students.Contains(student))
            {
                var processedMessage = new SignUpProcessedMessage(courseId, student, Status: SignUpStatus.New);
                var waitListOrder = await _coursesServiceProxy.ConsumeSeatAvailable(course, student);

                var status = waitListOrder switch
                {
                    0 => SignUpStatus.SeatReserved,
                    > 0 => SignUpStatus.OnWaitList,
                    _ => throw new InvalidOperationException("Unexpected value returned from consume seat available")
                };

                processedMessage = processedMessage with { Status = status };

                await _messageProxy.SendToTopic(Constants.SIGN_UP_PROCESSED_TOPIC, processedMessage);
            }
            else
            {
                // Student already on course. No need to process message
                return;
            }

        }


        // TODO: add message id to rollback to avoid refusing a student forever 
        [FunctionName("SignUpRollback")]
        public async System.Threading.Tasks.Task SignUpRollback([ServiceBusTrigger(Constants.ROLL_BACK_SEAT_RESERVATION, Constants.ALL_MESSAGES_SUBSCRIPTION, Connection = Constants.SERVICE_BUS_CONNECTION_NAME)] SignUpProcessedMessage message, ILogger log)
        {
            log.LogInformation($"[INF-0003] Rollback received: {message}");

            var (courseId, student, _) = message;
            var course = await _coursesServiceProxy.FindAsync(courseId);

            if (course == null)
            {
                log.LogCritical("[CRIT-0001] Course not found.");
                return;
            }

            if (course.Students.Contains(student))
            {
                await _coursesServiceProxy.ReleaseSeat(course, student);
                await _messageProxy.SendToTopic(Constants.NEW_SEAT_AVAILABLE, new NewSeatAvailableMessage(course.Id));
                await _messageProxy.SendToTopic(Constants.SIGN_UP_FINISHED, message);
            }
            else
            {
                // Student already rolledback. No need to process message
                return;
            }

        }

        [FunctionName("SignUpRollback")]
        public async System.Threading.Tasks.Task ConsumeNewSeat([ServiceBusTrigger(Constants.NEW_SEAT_AVAILABLE, Constants.ALL_MESSAGES_SUBSCRIPTION, Connection = Constants.SERVICE_BUS_CONNECTION_NAME)] NewSeatAvailableMessage message, ILogger log)
        {
            log.LogInformation($"[INF-0004] New seat available received: {message}");

            var courseId = message.CourseId;
            var student = await _coursesServiceProxy.GetNextInWaitList(courseId);
            var course = await _coursesServiceProxy.FindAsync(courseId);

            if (student == null)
            {
                log.LogInformation("[INF-0005] No students waiting");
                return;
            }


            if (!course.Students.Contains(student))
            {
                var processedMessage = new SignUpProcessedMessage(courseId, student, Status: SignUpStatus.New);
                var waitListOrder = await _coursesServiceProxy.ConsumeSeatAvailable(course, student);

                var status = waitListOrder switch
                {
                    0 => SignUpStatus.SeatReserved,
                    > 0 => SignUpStatus.OnWaitList,
                    _ => throw new InvalidOperationException("Unexpected value returned from consume seat available")
                };

                processedMessage = processedMessage with { Status = status };

                await _messageProxy.SendToTopic(Constants.SIGN_UP_PROCESSED_TOPIC, processedMessage);
            }
            else
            {
                // Student already on course. No need to process message
                return;
            }

        }



    }

    public record NewSeatAvailableMessage(string CourseId);
}
