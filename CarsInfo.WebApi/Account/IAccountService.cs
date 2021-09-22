using System.Threading.Tasks;
using CarsInfo.WebApi.Account.Models;

namespace CarsInfo.WebApi.Account
{
    public interface IAccountService
    {
        Task SendEmailVerificationAsync(EmailVerificationModel model);
    }
}