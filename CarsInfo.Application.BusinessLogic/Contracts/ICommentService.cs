using System.Collections.Generic;
using System.Threading.Tasks;
using CarsInfo.Application.BusinessLogic.Dtos;
using CarsInfo.Application.BusinessLogic.OperationResult;

namespace CarsInfo.Application.BusinessLogic.Contracts
{
    public interface ICommentService
    {
        Task<OperationResult.OperationResult> AddAsync(CommentEditorDto commentDto);
        
        Task<OperationResult<IEnumerable<CommentDto>>> GetByCarIdAsync(int carId);
    }
}
