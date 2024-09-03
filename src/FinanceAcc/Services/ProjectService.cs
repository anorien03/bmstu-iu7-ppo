using System;
using FinanceAcc.IService;
using FinanceAcc.Models;
using FinanceAcc.IRepository;
using FinanceAcc.Models.UserLevelLimits;
using FinanceAcc.Exceptions.ProjectServiceExceptions;
using FinanceAcc.Exceptions.AuthServiceExeptions;

namespace FinanceAcc.Services
{
	public class ProjectService: IProjectService
	{
		private readonly IProjectRepository _projectRepository;
        private readonly IProjectMemberRepository _projectMemberRepository;
        private readonly IUserRepository _userRepository;

        public ProjectService(IProjectRepository projectRepository, IProjectMemberRepository projectMemberRepository, IUserRepository userRepository)
		{
			_projectRepository = projectRepository ?? throw new ArgumentNullException();
            _projectMemberRepository = projectMemberRepository ?? throw new ArgumentNullException();
            _userRepository = userRepository ?? throw new ArgumentNullException();
        }


        private async Task<bool> CheckUserLimit(User user, MemberStatus status)
        {
            var limit = new UserLevelLimit().getLimit(user.Level, status);

            if (limit < 0)
            {
                return true;
            }

            var userProjectsCount = await _projectMemberRepository.CountRowsAsync(user.Id, status);
            if (userProjectsCount < limit)
            {
                return true;
            }
            return false;
        }


        public async Task CreateProjectAsync(User user, Project project)
        {
            if (! await CheckUserLimit(user, MemberStatus.Admin))
            {
                throw new UserLevelLimitReachedException("User reached the limit of owning projects.");
            }

            var newMember = new ProjectMember(user.Id, project.Id, MemberStatus.Admin);
            await _projectRepository.AddAsync(project);
            await _projectMemberRepository.AddAsync(newMember);
        }


        public async Task DeleteProjectAsync(int userId, int projectId)
        {
            var member = await _projectMemberRepository.GetByIdAsync(userId, projectId);
            if (member == null)
                throw new UserNotInvitedToProjectException("User isn't member of project");

            if (member.Status != MemberStatus.Admin)
                throw new AccessDeniedException("User isn't allowed to delete the project.");

            await _projectRepository.RemoveAsync(projectId);
        }


        public async Task<List<ProjectDetailed>> GetProjectsByUserIdAsync(int userId)
        {
            if (await _userRepository.GetByIdAsync(userId) == null)
            {
                throw new UserNotFoundException($"User with id {userId} not found");
            }

            var projectMembers = await _projectMemberRepository.GetRangeByUserIdAsync(userId);

            var projects = new List<ProjectDetailed>();

            for (int i = 0; i < projectMembers.Count(); i++)
            {
                projects.Add((await _projectRepository.GetByIdAsync(projectMembers[i].ProjectId)).ConvertToProjectDetailed(projectMembers[i].Status));
            }

            return projects;
        }

        

        public async Task<List<User>> GetUsersByProjectIdAsync(int projectId)
        {
            if(await _projectRepository.GetByIdAsync(projectId) == null)
            {
                throw new ProjectNotFoundException($"Project with id {projectId} not found");
            }

            var projectMembers = await _projectMemberRepository.GetRangeByProjectIdAsync(projectId);

            var users = new List<User>();

            foreach (var member in projectMembers)
            {
                if (member.Status != MemberStatus.Invited)
                {
                    users.Add(await _userRepository.GetByIdAsync(member.UserId));
                }
            }

            return users;
        }


        public async Task InviteUserToProjectAsync(int userId, int projectId)
        {
            if (await _userRepository.GetByIdAsync(userId) == null)
            {
                throw new UserNotFoundException($"User with id {userId} not found");
            }
            if (await _projectRepository.GetByIdAsync(projectId) == null)
            {
                throw new ProjectNotFoundException($"Project with id {projectId} not found");
            }

            if (await _projectMemberRepository.GetByIdAsync(userId, projectId) != null)
            {
                throw new UserAlreadyInvitedException($"User has already been invited.");
            }

            var newMember = new ProjectMember(userId, projectId, MemberStatus.Invited);
            await _projectMemberRepository.AddAsync(newMember);
        }


        public async Task AcceptInvitingToProjectAsync(User user, int projectId)
        {
            var member = await _projectMemberRepository.GetByIdAsync(user.Id, projectId);
            if (member == null)
            {
                throw new UserNotInvitedToProjectException("User hasn't been invited to project");
            }

            if (member.Status != MemberStatus.Invited)
            {
                throw new UserHasAlreadyJoinedException("User has already joined the project");
            }

            if (!await CheckUserLimit(user, MemberStatus.Member))
            {
                throw new UserLevelLimitReachedException("User reached the limit of sharing projects");
            }

            member.Status = MemberStatus.Member;
            await _projectMemberRepository.UpdateAsync(member);
        }


        public async Task RefuseInvitingToProjectAsync(int userId, int projectId)
        {
            var member = await _projectMemberRepository.GetByIdAsync(userId, projectId);
            if (member == null)
            {
                throw new UserNotInvitedToProjectException("User hasn't been invited to project.");
            }

            if (member.Status != MemberStatus.Invited)
            {
                throw new UserHasAlreadyJoinedException("User has already accepted the nvitation.");
            }

            await _projectMemberRepository.RemoveAsync(userId, projectId);
        }


        public async Task RemoveUserFromProjectAsync(int userId, int projectId)
        {
            var member = await _projectMemberRepository.GetByIdAsync(userId, projectId);
            if (member == null)
            {
                throw new UserNotInvitedToProjectException("User isn't member of the project.");
            }

            if (member.Status == MemberStatus.Admin)
            {
                throw new UnableRemoveAdminException("Unable to remove admin from the project");
            }

            await _projectMemberRepository.RemoveAsync(userId, projectId);
        }
    }
}

