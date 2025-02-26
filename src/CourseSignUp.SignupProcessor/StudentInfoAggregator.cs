using System;
using System.Collections.Immutable;
using System.Linq;
using CourseSignUp.Domain;
using CourseSignUp.Domain.Services;
using CourseSignUp.Domain.Services.Abstractions;
using CourseSignUp.Infraestructure.Abstractions;
using CourseSignUp.Infraestructure.Messages;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace CourseSignUp.MessageProcessors
{
    public class StudentInfoAggregator
    {
        private readonly IDistributedCacheService _cacheService;
        private readonly ICoursesService _coursesService;
        private readonly IStatisticsService _statisticsService;

        public StudentInfoAggregator(IDistributedCacheService cacheService, ICoursesService coursesService, IStatisticsService statisticsService)
        {
            _cacheService = cacheService;
            _coursesService = coursesService;
            _statisticsService = statisticsService;
        }

        [FunctionName("StudentInfoAggregator")]
        public async System.Threading.Tasks.Task RunAsync(
            [TimerTrigger("0 */5 * * * *")] TimerInfo info,
            ILogger log)
        {
            var now = DateTime.Now;
            log.LogInformation($"Calculating student aggregate info at: {now}");

            var batchedMessages = await _cacheService.ConsumeAllStored<SignUpProcessedMessage>(Constants.SIGN_UP_PROCESSED_TOPIC);
            var rolledBackMessages = await _cacheService.ConsumeAllStored<SignUpProcessedMessage>(Constants.ROLL_BACK_SEAT_RESERVATION);

            if (batchedMessages.Any(msg => !msg.StudentAccepted))
            {
                log.LogWarning("Only messages of accepted students should be stored", batchedMessages);
            }
            // TODO: add message id to rollback to avoid refusing a student forever 
            var rolledBackStudents = rolledBackMessages.Select(msg => msg.Student);
            var newStudentsPerCourse = (from msg in batchedMessages
                                        let student = msg.Student
                                        where !rolledBackStudents.Contains(student)
                                        group msg.Student by msg.CourseId);

                                       //where msg.StudentAccepted == t

            newStudentsPerCourse.AsParallel().ForAll(async newStudents =>
            {
                var course = await _coursesService.FindAsync(newStudents.Key);
                await _statisticsService.AggregateStatistics(course, newStudents.ToImmutableArray(), now);
            });
        }
    }
}
