using System;
using FinanceAcc.Models;

namespace FinanceAcc.IService
{
	public interface IRecordService
	{
		Task<List<Record>> GetRecordsByProjectIdAsync(Guid projectId);

        Task<List<Record>> GetRecordsByFilterAsync(Guid projectId, Guid userId, Guid categoryId, DateTime start, DateTime end);

        Task CreateRecordAsync(Record record);
    }
}

