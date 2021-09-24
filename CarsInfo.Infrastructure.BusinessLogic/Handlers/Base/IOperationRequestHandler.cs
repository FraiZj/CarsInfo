using CarsInfo.Application.BusinessLogic.Commands.Base;
using CarsInfo.Application.BusinessLogic.OperationResult;
using MediatR;

namespace CarsInfo.Infrastructure.BusinessLogic.Handlers.Base
{
    public interface IOperationResultRequestHandler<in TRequest> : IRequestHandler<TRequest, OperationResult>
        where TRequest : IOperationResultRequest
    {
    }
    
    public interface IOperationResultRequestHandler<in TRequest, TResponseType>
        : IRequestHandler<TRequest, OperationResult<TResponseType>>
        where TRequest : IOperationResultRequest<TResponseType>
    {
    }
}