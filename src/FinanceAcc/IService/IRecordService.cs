using System;
using System.Linq.Expressions;
using FinanceAcc.Models;

namespace FinanceAcc.IService
{
	public interface IRecordService
	{
        Task<List<Record>> GetRecordsAsync(int projectId, int? categoryId);

        Task AddRecordAsync(Record record);

        Task RemoveRecordAsync(int recordId);
    }
}

