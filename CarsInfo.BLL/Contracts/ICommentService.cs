using System.Threading.Tasks;
using CarsInfo.BLL.Models.Dtos;

namespace CarsInfo.BLL.Contracts
{
    public interface ICommentService
    {
        Task AddAsync(CommentDto commentDto);
        Task GetByCarIdAsync(int carId);
    }
}
