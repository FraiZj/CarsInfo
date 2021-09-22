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
        private readonly SendGridOptions _sendGridOptions;
        
        public SendGridEmailSender(IOptions<SendGridOptions> sendGridOptions)
        {
            _sendGridOptions = sendGridOptions.Value;
        }
        
        public Task SendEmailAsync(EmailModel emailModel, string senderName = Sender)
        {
            var client = new SendGridClient(_sendGridOptions.SendGridKey);
            var msg = new SendGridMessage
            {
                From = new EmailAddress(Sender, senderName),
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