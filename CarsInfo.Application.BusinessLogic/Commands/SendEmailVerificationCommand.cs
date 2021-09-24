using CarsInfo.Application.BusinessLogic.Commands.Base;
using CarsInfo.Application.BusinessLogic.Models;

namespace CarsInfo.Application.BusinessLogic.Commands
{
    public class SendEmailVerificationCommand : IOperationResultRequest
    {
        public EmailBodyModel EmailBodyModel { get; set; }
    }
}