using System;
namespace FinanceAcc.IRepository
{
	public interface IBaseRepository<T>
	{
		Task AddAsync(T entity);

        Task UpdateAsync(T entity);

        Task RemoveAsync(int id);
	}
}

