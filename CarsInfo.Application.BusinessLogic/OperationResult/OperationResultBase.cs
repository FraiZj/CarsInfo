using System;

namespace CarsInfo.Application.BusinessLogic.OperationResult
{
    public abstract class OperationResultBase
    {
        protected OperationResultBase()
        {
            Success = true;
        }
        
        protected OperationResultBase(string message)
        {
            FailureMessage = message;
        }
        
        protected OperationResultBase(Exception exception)
        {
            Exception = exception;
        }
        
        public bool Success { get; protected set; }
        public string FailureMessage { get; protected set; }
        public Exception Exception { get; protected set; }
        public bool IsException => Exception != null;
    }
}