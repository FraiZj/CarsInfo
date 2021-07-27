using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarsInfo.DAL.Contracts
{
	public interface IDbContext
	{
		Task AddAsync();

		Task UpdateAsync();

		Task DeleteAsync();

		Task<T> GetAsync<T>();

		Task<IEnumerable<T>> GetAllAsync<T>();
	}
}
