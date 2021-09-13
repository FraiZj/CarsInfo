using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarsInfo.Application.Persistence.Contracts;
using CarsInfo.Domain.Entities;

namespace CarsInfo.Infrastructure.Persistence.Repositories
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

            var users = await Context.QueryAsync<User, Role>(sql,
                (user, role) =>
                {
                    user.Roles.Add(role);
                    return user;
                });

            return GroupSet(users);
        }

        public async Task<User> GetWithRolesAsync(string email)
        {
            var sql = @$"SELECT u.*, Role.* FROM [{TableName}] u
                         INNER JOIN UserRole 
                         ON u.Id = UserRole.UserId
                         INNER JOIN Role
                         ON UserRole.RoleId = Role.Id
                         WHERE u.Email = '{email}'";

            var users = (await Context.QueryAsync<User, Role>(sql,
                (user, role) =>
                {
                    user.Roles.Add(role);
                    return user;
                })).ToList();

            return users.Count == 0 ? null : GroupSet(users).FirstOrDefault();
        }

        private IEnumerable<User> GroupSet(IEnumerable<User> users)
        {
            return users.GroupBy(u => u.Id).Select(g =>
            {
                var groupedUser = g.First();
                if (!groupedUser.Roles.Any())
                {
                    groupedUser.Roles = new List<Role>();
                }

                foreach (var user in g.Skip(1))
                {
                    if (user.Roles.Any())
                    {
                        groupedUser.Roles.Add(user.Roles.First());
                    }
                }

                return groupedUser;
            });
        }
    }
}
