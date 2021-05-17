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

            if (batchedMessages.Any(msg => !msg.StudentAccepted))
            {
                log.LogWarning("Only messages of accepted students should be stored", batchedMessages);
            }

            var newStudentsPerCourse = (from msg in batchedMessages
                                       group msg.Student by msg.CourseId);

                                       //where msg.StudentAccepted == t

            newStudentsPerCourse.AsParallel().ForAll(newStudents =>
            {
                var course = _coursesService.FindAsync(newStudents.Key);
                _statisticsService.AggregateStatistics(course, newStudents, now);
            });
        }
    }
}
