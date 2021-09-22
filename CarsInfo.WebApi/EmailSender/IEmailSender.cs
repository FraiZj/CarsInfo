using System.Threading.Tasks;
using CarsInfo.WebApi.EmailSender.Models;

namespace CarsInfo.WebApi.EmailSender
{
    public interface IEmailSender
    {
        Task SendEmailAsync(EmailModel emailModel);
    }
}