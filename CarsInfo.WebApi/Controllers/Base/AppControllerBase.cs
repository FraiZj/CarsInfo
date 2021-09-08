using Microsoft.AspNetCore.Mvc;

namespace CarsInfo.WebApi.Controllers.Base
{
    public abstract class AppControllerBase : ControllerBase
    {
        [NonAction]
        public virtual BadRequestObjectResult ApplicationError()
        {
            return ApplicationError("An error occured");
        }
        
        [NonAction]
        public virtual BadRequestObjectResult ApplicationError(string error)
        {
            return BadRequest(error);
        }
    }
}