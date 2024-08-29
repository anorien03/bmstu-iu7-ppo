using System;
using FinanceAcc.Models;

namespace FinanceAcc.IRepository
{
	public interface ICategory: IBaseRepository<Category>
	{
		Task<List<Category>> GetCategoriesByProjectIdAsync(int projectId);
	}
}

