using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarsInfo.DAL.Entities;

namespace CarsInfo.DAL.Contracts
{
	public interface IContext
	{
		Task AddAsync<T>(T entity) where T : BaseEntity;

		Task UpdateAsync<T>(T entity) where T : BaseEntity;

		Task DeleteAsync<T>(int id) where T : BaseEntity;

		Task<T> GetAsync<T>() where T : BaseEntity;

		Task<IEnumerable<T>> GetAllAsync<T>(IEnumerable<Type> includes) where T : BaseEntity;
	}
}
