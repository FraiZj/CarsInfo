using System.Collections.Generic;
using System.Threading.Tasks;
using CarsInfo.DAL.Contracts;
using CarsInfo.DAL.Entities;

namespace CarsInfo.DAL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly IContext _context;

        public GenericRepository(IContext context)
        {
            _context = context;
        }

        public Task AddAsync(T entity)
        {
            return _context.AddAsync(entity);
        }

        public Task DeleteAsync(int id)
        {
            return _context.DeleteAsync<T>(id);
        }

        public Task<IEnumerable<T>> GetAllAsync()
        {
            return _context.GetAllAsync<T>();
        }

        public Task<T> GetAsync(int id)
        {
            return _context.GetAsync<T>(id);
        }

        public Task UpdateAsync(T entity)
        {
            return _context.UpdateAsync(entity);
        }
    }
}
