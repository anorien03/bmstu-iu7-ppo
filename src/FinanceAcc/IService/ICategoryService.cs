using System;
using FinanceAcc.Models;

namespace FinanceAcc.IService
{
	public interface ICategoryService
	{
		Task<List<Category>> GetCategoriesByProjectIdAsync(Guid projectId);
	}
}

