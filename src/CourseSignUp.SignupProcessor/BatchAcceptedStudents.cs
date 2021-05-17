using CourseSignUp.Domain;
using CourseSignUp.Infraestructure.Abstractions;
using CourseSignUp.Infraestructure.Messages;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace CourseSignUp.MessageProcessors
{
    public class BatchAcceptedStudents
    {
        private readonly IDistributedCacheService _cacheService;

        public BatchAcceptedStudents(IDistributedCacheService cacheService)
        {
            _cacheService = cacheService;
        }

        [FunctionName("BatchAcceptedStudents")]
        public async System.Threading.Tasks.Task RunAsync([ServiceBusTrigger(Constants.SIGN_UP_PROCESSED_TOPIC, Constants.ACCEPTED_STUDENTS_MESSAGES, Connection = Constants.SERVICE_BUS_CONNECTION_NAME)] SignUpProcessedMessage message, ILogger log)
        {
            var studentAccepted = message.StudentAccepted;
            if (studentAccepted)
            {
                // Storing messages on cache to process all together
                await _cacheService.StoreOnSet(Constants.SIGN_UP_PROCESSED_TOPIC, message);
            }
        }
    }
}
