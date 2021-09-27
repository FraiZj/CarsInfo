using System.Collections.Generic;
using System.Threading.Tasks;
using CarsInfo.Application.Persistence.Contracts;
using CarsInfo.Application.Persistence.Filters;
using CarsInfo.Domain.Entities;
using CarsInfo.Infrastructure.Persistence.Configurators;

namespace CarsInfo.Infrastructure.Persistence.Repositories
{
    public class CommentRepository : GenericRepository<Comment>
    {
        public CommentRepository(IDbContext context) 
            : base(context)
        { }

        public override async Task<IEnumerable<Comment>> GetAllAsync(FilterModel filterModel)
        {
            var filters = SqlQueryConfigurator.ConfigureFilter(
                TableName, filterModel.Filters, filterModel.IncludeDeleted);
            var orderBy = SqlQueryConfigurator.ConfigureOrderBy(filterModel.OrderBy);

            var sql = $@"SELECT * FROM Comment
                        LEFT JOIN [User]
                        ON Comment.UserId = [User].Id
                        { filters }
                        { orderBy }
                        OFFSET { filterModel.Skip } ROWS
                        FETCH NEXT { filterModel.Take } ROWS ONLY";


            return await Context.QueryAsync<Comment, User>(sql, (comment, user) =>
            {
                comment.User = user;
                return comment;
            });
        }
    }
}