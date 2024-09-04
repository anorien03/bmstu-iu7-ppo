using System;
using FinanceAcc.Models;

namespace FinanceAcc.IRepository
{
	public interface ICategoryRepository
	{
		Task<List<Category>> GetRangeByProjectIdAsync(int projectId);

        Task<Category> GetByIdAsync(int categoryId);

        Task AddRangeAsync(List<Category> categories);
	}
}

