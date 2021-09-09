using System;

namespace CarsInfo.Application.BusinessLogic.OperationResult
{
    public class OperationResult<T> : OperationResultBase
    {
        protected OperationResult(T result)
        {
            Result = result;
        }
        
        protected OperationResult(string message)
            : base(message)
        { }

        public T Result { get; set; }
        
        public static OperationResult<T> SuccessResult(T result)
        {
            return new OperationResult<T>(result);
        }
        
        public static OperationResult<T> FailureResult(string message)
        {
            return new OperationResult<T>(message);
        }
        
        public static OperationResult<T> ExceptionResult()
        {
            return new OperationResult<T>("An error occurred, please try again later");
        }
    }
}