using System.Collections.Generic;
using System.Threading.Tasks;
using CarsInfo.DAL.Contracts;

namespace CarsInfo.DAL
{
	public class JsonContext : IDbContext
	{
		public JsonContext()
		{

		}

		public Task AddAsync()
		{
			throw new System.NotImplementedException();
		}

		public Task DeleteAsync()
		{
			throw new System.NotImplementedException();
		}

		public Task<IEnumerable<T>> GetAllAsync<T>()
		{
			throw new System.NotImplementedException();
		}

		public Task<T> GetAsync<T>()
		{
			throw new System.NotImplementedException();
		}

		public Task UpdateAsync()
		{
			throw new System.NotImplementedException();
		}
	}
}
