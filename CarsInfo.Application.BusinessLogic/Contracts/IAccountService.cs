using System.Threading.Tasks;
using CarsInfo.Application.BusinessLogic.Models;

namespace CarsInfo.Application.BusinessLogic.Contracts
{
    public interface IAccountService
    {
        Task<OperationResult.OperationResult> SendEmailVerificationAsync(EmailBodyModel model);
        
        Task<OperationResult.OperationResult> SendResetPasswordEmailAsync(EmailBodyModel model);
    }
}