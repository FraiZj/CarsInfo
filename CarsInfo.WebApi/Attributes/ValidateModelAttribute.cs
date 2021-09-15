using System;
using System.Linq;
using System.Threading.Tasks;
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
                var errors = context.ModelState
                    .Where(x => x.Value.Errors.Any())
                    .ToDictionary(
                        x => x.Key,
                        x => x.Value.Errors.Select(e => e.ErrorMessage))
                    .ToArray()
                    .SelectMany(modelStateError  => modelStateError.Value
                        .Select(value => new ErrorModel
                        {
                            Field = modelStateError.Key,
                            Error = value
                        }));

                context.Result = new BadRequestObjectResult(new ErrorResponse(errors));
                return;
            }

            await next();
        }
    }
}
