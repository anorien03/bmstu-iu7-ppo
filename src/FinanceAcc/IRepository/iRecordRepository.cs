using System;
using System.Linq.Expressions;
using FinanceAcc.Models;

namespace FinanceAcc.IRepository
{
	public interface IRecord: IBaseRepository<Record>
	{
		Task<List<Record>> GetFilteredRecordsAsync(Expression<Func<int, int>> operfilter);
	}
}

