using System;
using System.Linq.Expressions;
using FinanceAcc.Models;

namespace FinanceAcc.IRepository
{
	public interface IProjectMemberRepository
    {
        Task AddAsync(ProjectMember member);

        Task UpdateAsync(ProjectMember member);

        Task RemoveAsync(int userId, int projectId);


        Task<List<ProjectMember>> GetRangeByUserIdAsync(int userId);

        Task<List<ProjectMember>> GetRangeByProjectIdAsync(int projectId);

        Task<int> CountRowsAsync(int userId, MemberStatus status);

        Task<ProjectMember> GetByIdAsync(int userId, int projectId);
    }
}

