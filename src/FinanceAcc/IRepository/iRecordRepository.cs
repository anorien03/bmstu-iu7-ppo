using System;
using System.Linq.Expressions;
using FinanceAcc.Models;

namespace FinanceAcc.IRepository
{
	public interface IRecordRepository: IBaseRepository<Record>
	{
        Task<Record> GetByIdAsync(int recordId);

        Task<List<Record>> GetFilteredAsync(Expression<Func<Record, bool>> filter);
	}
}

