using System.Collections.Generic;
using System.Threading.Tasks;
using CarsInfo.DAL.Contracts;
using CarsInfo.DAL.Entities;

namespace CarsInfo.DAL.Repositories
{
    public class UsersRepository : GenericRepository<User>, IUsersRepository
    {
        public UsersRepository(IDbContext context) 
            : base(context)
        { }

        public async Task<IEnumerable<User>> GetAllWithRolesAsync()
        {
            var sql = @$"SELECT * FROM {TableName} user
                         INNER JOIN UserRole
                         ON user.UserId = UserRole.UserId
                         INNER JOIN Role
                         ON UserRole.RoleId = Role.Id";

            return await Context.QueryAsync<User, Role>(sql,
                (user, role) =>
                {
                    user.Roles.Add(role);
                    return user;
                });
        }

        public async Task<User> GetWithRolesAsync(string email)
        {
            var sql = @$"SELECT TOP 1 * FROM [{TableName}] u
                         INNER JOIN UserRole 
                         ON u.Id = UserRole.UserId
                         INNER JOIN [Role]
                         ON UserRole.RoleId = [Role].Id
                         WHERE u.Email = '{email}'";

            return await Context.QueryFirstOrDefaultAsync<User, Role>(sql,
                (user, role) =>
                {
                    if (role is not null)
                    {
                        user.Roles.Add(role);
                    }

                    return user;
                });
        }
    }
}
