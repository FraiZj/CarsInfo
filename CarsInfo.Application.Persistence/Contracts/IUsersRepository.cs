using System.Collections.Generic;
using System.Threading.Tasks;
using CarsInfo.Domain.Entities;

namespace CarsInfo.Application.Persistence.Contracts
{
    public interface IUsersRepository : IGenericRepository<User>
    {
        Task<IEnumerable<User>> GetAllWithRolesAsync();
        Task<User> GetWithRolesAsync(string email);
    }
}
