using System.Collections.Generic;
using System.Threading.Tasks;
using CarsInfo.DAL.Entities;

namespace CarsInfo.DAL.Contracts
{
    public interface IUsersRepository : IGenericRepository<User>
    {
        Task<IEnumerable<User>> GetAllWithRolesAsync();
        Task<User> GetWithRolesAsync(string email);
    }
}
