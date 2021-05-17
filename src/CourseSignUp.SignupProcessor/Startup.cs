using CourseSignUp.Infraestructure;
using CourseSignUp.Infraestructure.Abstractions;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(CourseSignUp.MessageProcessors.Startup))]
namespace CourseSignUp.MessageProcessors
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddCourseSignUp();
        }
    }
}
