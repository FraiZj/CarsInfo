using System.Collections.Generic;
using System.Threading.Tasks;
using CarsInfo.Application.BusinessLogic.Contracts;
using CarsInfo.Application.Persistence.Contracts;
using CarsInfo.Application.Persistence.Filters;
using CarsInfo.Domain.Entities;

namespace CarsInfo.Infrastructure.BusinessLogic.Services
{
    public class RoleService : IRoleService
    {
        private readonly IGenericRepository<Role> _roleRepository;

        public RoleService(IGenericRepository<Role> roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<int> GetRoleIdAsync(string roleName)
        {
            var role = await _roleRepository.GetAsync(new List<FiltrationField>
            {
                new("Name", roleName)
            });

            return role.Id;
        }
    }
}
