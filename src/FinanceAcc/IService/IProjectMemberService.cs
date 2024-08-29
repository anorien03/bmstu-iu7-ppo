using System;
using FinanceAcc.Models;

namespace FinanceAcc.IService
{
	public interface IProjectMemberService
	{
        Task InviteUserToProjectAsync(Guid projectId, Guid userId);

        Task AcceptInvitingToProjectAsync(Guid projectId, Guid userId);

        Task RefuseinvitingToProjectAsync(Guid projectId, Guid userId);

        Task DeleteProjectMember(ProjectMember member);
    }
}

