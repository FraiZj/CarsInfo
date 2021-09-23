﻿using System.Threading.Tasks;
using CarsInfo.Application.BusinessLogic.OperationResult;
using CarsInfo.WebApi.Account.Models;

namespace CarsInfo.WebApi.Account
{
    public interface IAccountService
    {
        Task<OperationResult> SendEmailVerificationAsync(EmailVerificationModel model);
    }
}