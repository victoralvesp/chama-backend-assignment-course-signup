using CourseSignUp.Domain.Extensions;
using CourseSignUp.Infraestructure.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace CourseSignUp.Infraestructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCourseSignUp(this IServiceCollection services)
        {
            // TODO : add courses, lecturers, statistics repository as MongoDB repositories
            //services.AddScoped<ICoursesRepository, MongoDBCoursesRepository>(); 
            //services.AddScoped<IStatisticsRepository, MongoDBStatisticsRepository>(); 
            //services.AddScoped<ILecturerRepository, MongoDBLecturerRepository>();
            // TODO: add message service as Azure Service Bus sender
            //services.AddScoped<IMessageBusService, AzureMessageBusService>();

            // TODO : add Redis as distributed cache service
            //services.AddScoped<IDistributedCacheService, RedisService>();

            // TODO : add email service
            //services.AddScoped<IEmailService, OutlookEmailService>();
            AddMocks(services);
            services.AddCourseSignUpDomain();

            return services;
        }

        private static void AddMocks(IServiceCollection services)
        {
            services.AddScoped((serv) => Mock.Of<ICoursesRepository>(MockBehavior.Loose));
            services.AddScoped((serv) => Mock.Of<ILecturerRepository>(MockBehavior.Loose));
            services.AddScoped((serv) => Mock.Of<IStatisticsRepository>(MockBehavior.Loose));
            services.AddScoped((serv) => Mock.Of<IMessageBusService>(MockBehavior.Loose));
            services.AddScoped((serv) => Mock.Of<IEmailService>(MockBehavior.Loose));
            services.AddScoped((serv) => Mock.Of<IDistributedCacheService>(MockBehavior.Loose));
        }
    }
}
