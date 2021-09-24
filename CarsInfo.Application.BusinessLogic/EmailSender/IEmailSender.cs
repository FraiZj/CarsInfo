using System.Threading.Tasks;
using CarsInfo.Application.BusinessLogic.EmailSender.Models;

namespace CarsInfo.Application.BusinessLogic.EmailSender
{
    public interface IEmailSender
    {
        Task SendEmailAsync(EmailModel emailModel);
    }
}