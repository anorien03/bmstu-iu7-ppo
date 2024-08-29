using System;
using FinanceAcc.Models;

namespace FinanceAcc.IRepository
{
	public interface IProjectMemberRepository: IBaseRepository<ProjectMember>
    {
		Task<List<ProjectMember>> GetProjectMembersByProjectIdAsync(int projectId);
	}
}

