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

        public override async Task<IEnumerable<User>> GetAllAsync()
        {
            const string selectUsers = @"SELECT * FROM [User]
                                         INNER JOIN [UserRole]
                                         ON [User].UserId = [UserRole].UserId
                                         INNER JOIN [Role]
                                         ON [UserRole].RoleId = [Role].Id";

            var users = await Context.QueryAsync<User, Role>(selectUsers,
                (user, role) =>
                {
                    user.Roles.Add(role);
                    return user;
                });

            return GroupSet(users);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            const string selectUserByEmail = @"SELECT [User].*, [Role].* 
                                                FROM [User]
                                                INNER JOIN [UserRole] 
                                                ON [User].Id = [UserRole].UserId
                                                INNER JOIN [Role]
                                                ON [UserRole].RoleId = [Role].Id
                                                WHERE [User].Email = @email";

            var users = (await Context.QueryAsync<User, Role>(
                selectUserByEmail,
                (user, role) =>
                {
                    user.Roles.Add(role);
                    return user;
                }, new
                {
                    email
                })).ToList();

            return users.Count == 0 ? null : GroupSet(users).FirstOrDefault();
        }

        private static IEnumerable<User> GroupSet(IEnumerable<User> users)
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
