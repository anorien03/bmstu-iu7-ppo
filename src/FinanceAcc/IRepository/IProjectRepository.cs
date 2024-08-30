using System;
using FinanceAcc.Models;

namespace FinanceAcc.IRepository
{
	public interface IProjectRepository: IBaseRepository<Project>
    {
        Task<Project> GetByIdAsync(int id);
    }
}
