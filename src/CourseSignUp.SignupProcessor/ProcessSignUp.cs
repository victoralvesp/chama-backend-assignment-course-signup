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
    public class ProcessSignUp
    {
        private readonly ICoursesService _coursesService;

        private readonly IMessageBusService _messageBusService;

        public ProcessSignUp(ICoursesService coursesService, IMessageBusService messageBusService)
        {
            _coursesService = coursesService;
            _messageBusService = messageBusService;
        }

        [FunctionName("ProcessSignUp")]
        public async System.Threading.Tasks.Task RunAsync([ServiceBusTrigger(Constants.NEW_SIGN_UP_TOPIC, Constants.ALL_MESSAGES_SUBSCRIPTION, Connection = "ServiceBusConnection")] NewSignUpMessage message, ILogger log)
        {
            log.LogInformation($"[INF-0001] New sign up received: {message}");

            var (courseId, student) = message;
            var course = await _coursesService.FindAsync(courseId);


            if (!course.Students.Contains(student))
            {
                var processedMessage = new SignUpProcessedMessage(courseId, student, StudentAccepted: false);
                if (await _coursesService.ConsumeSeatAvailable(course, student))
                {
                    processedMessage = processedMessage with { StudentAccepted = true };
                }
             
                await _messageBusService.SendToTopic(Constants.SIGN_UP_PROCESSED_TOPIC, processedMessage);
            }
            else
            {
                // Student already on course. No need to process message
                return;
            }

        }



    }

    
}
