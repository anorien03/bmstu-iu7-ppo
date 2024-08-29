using System;
namespace FinanceAcc.IRepository
{
	public interface IBaseRepository<T>
	{
		Task<T> GetAllAsync();

		Task<List<T>> GetByIdAsync(int id);

		Task AddAsync(T entity);

        Task UpdateAsync(T entity);

        Task RemoveAsync(T entity);
	}
}

