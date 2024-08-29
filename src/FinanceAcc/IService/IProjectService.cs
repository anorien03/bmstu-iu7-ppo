using System;
using FinanceAcc.Models;

namespace FinanceAcc.IService
{
	public interface IProjectService
	{
        Task<List<Project>> GetProjectsByUserIdAsync(Guid userId);

        Task<List<User>> GetUsersByProjectIdAsync(Guid projectId);

        Task CreateProjectAsync(Project project);

        Task DeleteProjectAsync(Guid projectId);
    }
}

