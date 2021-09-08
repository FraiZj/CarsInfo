using System;

namespace CarsInfo.Application.BusinessLogic.OperationResult
{
    public class OperationResult : OperationResultBase
    {
        protected OperationResult() { }

        protected OperationResult(string message)
            : base(message)
        { }
        
        protected OperationResult(Exception exception)
            : base(exception)
        { }

        public static OperationResult SuccessResult()
        {
            return new OperationResult();
        }
        
        public static OperationResult FailureResult(string message)
        {
            return new OperationResult(message);
        }
        
        public static OperationResult ExceptionResult(Exception exception)
        {
            return new OperationResult(exception);
        }
    }
}