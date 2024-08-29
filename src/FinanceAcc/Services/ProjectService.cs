using System;
using FinanceAcc.IService;
using FinanceAcc.Models;
using FinanceAcc.IRepository;

namespace FinanceAcc.Services
{
	public class ProjectService: IProjectService
	{
		private readonly IProjectRepository _projectRepository;
        private readonly IProjectMemberRepository _projectMemberRepository;

        public ProjectService(IProjectRepository projectRepository, IProjectMemberRepository projectmemberRepository)
		{
			_projectRepository = projectRepository ?? throw new ArgumentNullException();
            _projectMemberRepository = projectmemberRepository ?? throw new ArgumentNullException();

        }


        //private int CountUserProjects(User user, MemberStatus status)
        //{

        //}

        public Task CreateProjectAsync(User user, Project project)
        {
            throw new NotImplementedException();
        }

        public Task DeleteProjectAsync(Guid projectId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Project>> GetProjectsByUserIdAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<User>> GetUsersByProjectIdAsync(Guid projectId)
        {
            throw new NotImplementedException();
        }
    }
}

