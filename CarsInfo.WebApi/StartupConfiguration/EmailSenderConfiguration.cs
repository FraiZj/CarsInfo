using CarsInfo.Application.BusinessLogic.EmailSender;
using CarsInfo.Application.BusinessLogic.EmailSender.Options;
using CarsInfo.Infrastructure.BusinessLogic.EmailSender;
using CarsInfo.Infrastructure.BusinessLogic.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CarsInfo.WebApi.StartupConfiguration
{
    public static class EmailSenderConfiguration
    {
        public static void AddEmailSenderConfiguration(
            this IServiceCollection services, 
            IConfiguration configuration)
        {
            var sendGridOptions = new SendGridOptions();
            configuration.GetSection(nameof(SendGridOptions)).Bind(sendGridOptions);
            services.AddSingleton(sendGridOptions);
            
            var emailSenderOptions = new EmailSenderOptions();
            configuration.GetSection(nameof(EmailSenderOptions)).Bind(emailSenderOptions);
            services.AddSingleton(emailSenderOptions);
            services.AddTransient<IEmailSender, SendGridEmailSender>();
        }
    }
}