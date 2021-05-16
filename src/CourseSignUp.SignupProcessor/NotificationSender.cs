using System;
using CourseSignUp.Domain;
using CourseSignUp.Domain.Services.Abstractions;
using CourseSignUp.Infraestructure.Abstractions;
using CourseSignUp.Infraestructure.Messages;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace CourseSignUp.MessageProcessors
{
    public class NotificationSender
    {
        private readonly ICoursesService _coursesService;
        private readonly IEmailService _emailService;

        public NotificationSender(ICoursesService coursesService, IEmailService emailService)
        {
            _coursesService = coursesService;
            _emailService = emailService;
        }

        [FunctionName("NotificationSender")]
        public async System.Threading.Tasks.Task RunAsync([ServiceBusTrigger(Constants.SIGN_UP_PROCESSED_TOPIC, Constants.ALL_MESSAGES_SUBSCRIPTION, Connection = "ServiceBusConnection")]SignUpProcessedMessage message, ILogger log)
        {
            log.LogInformation($"Sending email for sign up: {message}");

            var (courseId, student, studentAccepted) = message;
            var course = await _coursesService.FindAsync(courseId);
            if (studentAccepted)
            {
                _emailService.SendEmail(student.Email, ChamaSystemTexts.AcceptedIntoCourseTemplate(course, student));
            }
            else
            {
                _emailService.SendEmail(student.Email, ChamaSystemTexts.NotAcceptedIntoCourseTemplate(course, student));
            }
        }
    }
}
