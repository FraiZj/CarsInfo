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
        
        public bool Success { get; protected set; }
        public string FailureMessage { get; protected set; }
    }
}