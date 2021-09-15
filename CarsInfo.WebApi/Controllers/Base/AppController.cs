using CarsInfo.WebApi.ViewModels.Error;
using Microsoft.AspNetCore.Mvc;

namespace CarsInfo.WebApi.Controllers.Base
{
    public abstract class AppController : ControllerBase
    {
        [NonAction]
        public virtual BadRequestObjectResult BadRequest(string applicationError)
        {
            return new BadRequestObjectResult(new ErrorResponse
            {
                ApplicationError = applicationError
            });
        }
    }
}
