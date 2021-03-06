using System.Threading.Tasks;
using CarsInfo.Domain.Entities;

namespace CarsInfo.Application.Persistence.Contracts
{
    public interface IUsersRepository : IGenericRepository<User>
    {
        Task<User> GetByEmailAsync(string email);
    }
}
