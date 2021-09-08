using System;
using CarsInfo.Application.BusinessLogic.OperationResult;
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

        protected virtual IActionResult PutResult(
            OperationResultBase operation,
            Func<IActionResult> onSuccess)
        {
            return PutAndDeleteResult(operation, onSuccess);
        }

        protected virtual IActionResult DeleteResult(
            OperationResultBase operation,
            Func<IActionResult> onSuccess)
        {
            return PutAndDeleteResult(operation, onSuccess);
        }
        
        private IActionResult PutAndDeleteResult(
            OperationResultBase operation, 
            Func<IActionResult> onSuccess)
        {
            if (operation.IsException)
            {
                return ApplicationError();
            }
            
            return operation.Success ? 
                onSuccess() :
                BadRequest(operation.FailureMessage);
        }
        
        public virtual IActionResult GetResult<T>(
            OperationResult<T> operation, 
            Func<T, IActionResult> onSuccess)
        {
            if (operation.IsException)
            {
                return ApplicationError();
            }
            
            return operation.Success ? 
                onSuccess(operation.Result) :
                BadRequest(operation.FailureMessage);
        }
    }
}