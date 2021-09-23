﻿using CarsInfo.WebApi.EmailSender;
using CarsInfo.WebApi.EmailSender.Options;
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
            var emailSenderOptions = new EmailSenderOptions();
            configuration.GetSection(nameof(EmailSenderOptions)).Bind(emailSenderOptions);
            services.AddSingleton(emailSenderOptions);
            services.AddTransient<IEmailSender, SendGridEmailSender>();
            services.Configure<SendGridOptions>(configuration);
        }
    }
}