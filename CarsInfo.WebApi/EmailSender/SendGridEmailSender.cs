using System.Threading.Tasks;
using CarsInfo.WebApi.EmailSender.Models;
using CarsInfo.WebApi.EmailSender.Options;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace CarsInfo.WebApi.EmailSender
{
    public class SendGridEmailSender : IEmailSender
    {
        private const string Sender = "daniil.khyzhniak@gmail.com";
        private const string SenderName = "CarsInfo";
        private readonly SendGridOptions _sendGridOptions;
        
        public SendGridEmailSender(IOptions<SendGridOptions> sendGridOptions)
        {
            _sendGridOptions = sendGridOptions.Value;
        }
        
        public Task SendEmailAsync(EmailModel emailModel)
        {
            var client = new SendGridClient(_sendGridOptions.SendGridKey);
            var msg = new SendGridMessage
            {
                From = new EmailAddress(Sender, SenderName),
                Subject = emailModel.Subject,
                PlainTextContent = emailModel.Message,
                HtmlContent = emailModel.Message
            };
            msg.AddTo(new EmailAddress(emailModel.Email));
            msg.SetClickTracking(false, false);

            return client.SendEmailAsync(msg);
        }
    }
}