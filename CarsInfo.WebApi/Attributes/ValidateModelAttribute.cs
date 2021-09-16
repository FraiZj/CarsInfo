using System;
using System.Threading.Tasks;
using CarsInfo.WebApi.Extensions;
using CarsInfo.WebApi.ViewModels.Error;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CarsInfo.WebApi.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ValidateModelAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState.GetErrorModels();
                context.Result = new BadRequestObjectResult(new ErrorResponse(errors));
                return;
            }

            await next();
        }
    }
}
