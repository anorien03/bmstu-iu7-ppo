using System;
using FinanceAcc.Models;

namespace FinanceAcc.IService
{
	public interface IProjectService
	{
        Task<List<ProjectDetailed>> GetProjectsByUserIdAsync(int userId);

        Task<List<User>> GetUsersByProjectIdAsync(int projectId);

        Task CreateProjectAsync(User user, Project project);

        Task DeleteProjectAsync(int userId, int projectId);

        Task InviteUserToProjectAsync(int userId, int projectId);

        Task AcceptInvitingToProjectAsync(User user, int projectId);

        Task RefuseInvitingToProjectAsync(int userId, int projectId);

        Task RemoveUserFromProjectAsync(int userId, int projectId);
    }
}

