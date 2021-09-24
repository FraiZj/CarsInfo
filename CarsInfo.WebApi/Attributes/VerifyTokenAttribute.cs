using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CarsInfo.Application.BusinessLogic.Contracts;
using CarsInfo.WebApi.ViewModels.Error;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace CarsInfo.WebApi.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class VerifyTokenAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var token = context.HttpContext.Request.Query["token"].ToString();
            var tokenService = context.HttpContext.RequestServices.GetRequiredService<ITokenService>();
            
            if (string.IsNullOrEmpty(token))
            {
                context.Result = new BadRequestObjectResult(new ErrorResponse
                {
                    ApplicationError = "Invalid token"
                });
                return;
            }

            var jwt = tokenService.DecodeJwtToken(token);

            if (DateTime.UtcNow > jwt.ValidTo)
            {
                context.Result = new BadRequestObjectResult(new ErrorResponse
                {
                    ApplicationError = "Token expired"
                });
                return;
            }
            
            if (jwt.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email) is null)
            {
                context.Result = new BadRequestObjectResult(new ErrorResponse
                {
                    ApplicationError = "Invalid email"
                });
                return;
            }

            await next();
        }
    }
}