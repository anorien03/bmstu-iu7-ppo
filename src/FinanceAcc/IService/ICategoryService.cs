using System;
using FinanceAcc.Models;

namespace FinanceAcc.IService
{
	public interface ICategoryService
	{
		Task<List<Category>> GetCategoriesByProjectIdAsync(int projectId);

		Task AddCategoriesToProjectAsync(int projectId);

    }
}

