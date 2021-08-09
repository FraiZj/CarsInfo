using System.Collections.Generic;
using System.Threading.Tasks;
using CarsInfo.DAL.Assistance;
using CarsInfo.DAL.Contracts;
using CarsInfo.DAL.Entities;

namespace CarsInfo.DAL.Repositories
{
    public class GenericJsonRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly IContext _context;

        public GenericJsonRepository(IContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(T entity)
        {
            await _context.AddAsync(entity);
            return 1;
        }

        public Task DeleteAsync(int id)
        {
            return _context.DeleteAsync<T>(id);
        }

        public Task<T> GetAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<T> GetAsync(ICollection<FilterModel> filters)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<T>> GetAllAsync()
        {
            return _context.GetAllAsync<T>();
        }

        public Task<IEnumerable<T>> GetAllAsync(ICollection<FilterModel> filters)
        {
            throw new System.NotImplementedException();
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
