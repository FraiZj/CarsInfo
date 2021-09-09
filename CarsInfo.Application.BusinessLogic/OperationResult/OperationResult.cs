using System;

namespace CarsInfo.Application.BusinessLogic.OperationResult
{
    public class OperationResult : OperationResultBase
    {
        protected OperationResult() { }

        protected OperationResult(string message)
            : base(message)
        { }

        public static OperationResult SuccessResult()
        {
            return new OperationResult();
        }
        
        public static OperationResult FailureResult(string message)
        {
            return new OperationResult(message);
        }
        
        public static OperationResult ExceptionResult()
        {
            return new OperationResult("An error occurred, please try again later");
        }
    }
}