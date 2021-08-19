using System.Threading.Tasks;
using CarsInfo.Application.BusinessLogic.Dtos;

namespace CarsInfo.Application.BusinessLogic.Contracts
{
    public interface ICommentService
    {
        Task AddAsync(CommentDto commentDto);
        Task GetByCarIdAsync(int carId);
    }
}
