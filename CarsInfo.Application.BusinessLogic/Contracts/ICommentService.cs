using System.Collections.Generic;
using System.Threading.Tasks;
using CarsInfo.Application.BusinessLogic.Dtos;

namespace CarsInfo.Application.BusinessLogic.Contracts
{
    public interface ICommentService
    {
        Task AddAsync(CommentEditorDto commentDto);
        Task<IEnumerable<CommentDto>> GetByCarIdAsync(int carId);
    }
}
