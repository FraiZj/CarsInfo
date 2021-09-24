using MediatR;

namespace CarsInfo.Application.BusinessLogic.Commands.Base
{
    public interface IOperationResultRequest : IRequest<OperationResult.OperationResult>
    {
    }

    public interface IOperationResultRequest<TResponseType> : IRequest<OperationResult.OperationResult<TResponseType>>
    {
    }
}