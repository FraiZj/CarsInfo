using System.Threading.Tasks;

namespace CarsInfo.Application.BusinessLogic.Contracts
{
    public interface IRoleService
    {
        Task<int> GetRoleIdAsync(string roleName);
    }
}
