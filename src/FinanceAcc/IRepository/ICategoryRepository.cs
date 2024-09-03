using System;
using FinanceAcc.Models;

namespace FinanceAcc.IRepository
{
	public interface ICategoryRepository
	{
		Task<List<Category>> GetRangeByProjectIdAsync(int projectId);

		Task AddRangeAsync(List<Category> categories);
	}
}

