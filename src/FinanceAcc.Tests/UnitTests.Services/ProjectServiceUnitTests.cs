using System;
using FinanceAcc.Exceptions.AuthServiceExeptions;
using FinanceAcc.Exceptions.ProjectServiceExceptions;
using FinanceAcc.IRepository;
using FinanceAcc.IService;
using FinanceAcc.Models;
using FinanceAcc.Services;
using Moq;
using Xunit;

namespace FinanceAcc.Tests.UnitTests.Services
{
	public class ProjectServiceUnitTests
    {
        private readonly IProjectService _projectService;
        private readonly Mock<IUserRepository> _mockUserRepository = new();
        private readonly Mock<IProjectRepository> _mockProjectRepository = new();
        private readonly Mock<IProjectMemberRepository> _mockProjectMemberRepository = new();

        public ProjectServiceUnitTests()
		{
            _projectService = new ProjectService(_mockProjectRepository.Object, _mockProjectMemberRepository.Object, _mockUserRepository.Object);
		}


        [Fact]
        public async void CreateProjectOkTest()
        {
            List<User> users = CreateListOfUsers();
            List<Project> projects = CreateListOfProjects();
            List<ProjectMember> members = CreateListOfProjectMembers();

            var user = users[1];
            var newProject = new Project(4, "King");
            
            _mockProjectMemberRepository.Setup(repo => repo.CountRowsAsync(user.Id, MemberStatus.Admin)).ReturnsAsync(members.Count(u => u.UserId == user.Id & u.Status == MemberStatus.Admin));
            _mockProjectMemberRepository.Setup(repo => repo.AddAsync(It.IsAny<ProjectMember>())).Callback((ProjectMember m) => members.Add(m));
            _mockProjectRepository.Setup(repo => repo.AddAsync(newProject)).Callback((Project p) => projects.Add(p));

            await _projectService.CreateProjectAsync(user, newProject);
            var actualProject = projects.Last();
            var actualMember = members.Last();

            Assert.Equal(newProject, actualProject);
            Assert.Equal(user.Id, actualMember.UserId);
            Assert.Equal(newProject.Id, actualMember.ProjectId);
            Assert.Equal(MemberStatus.Admin, actualMember.Status);
        }


        [Fact]
        public async void CreateProjectLimitReachedTest()
        {
            List<User> users = CreateListOfUsers();
            List<Project> projects = CreateListOfProjects();
            List<ProjectMember> members = CreateListOfProjectMembers();

            var user = users[0];
            var newProject = new Project(4, "King");

            _mockProjectMemberRepository.Setup(repo => repo.CountRowsAsync(user.Id, MemberStatus.Admin)).ReturnsAsync(members.Count(u => u.UserId == user.Id & u.Status == MemberStatus.Admin));
        

            async Task Result() => await _projectService.CreateProjectAsync(user, newProject);

            await Assert.ThrowsAsync<UserLevelLimitReachedException>(Result);
        }


        [Fact]
        public async void DeleteProjectOkTest()
        {
            List<Project> projects = CreateListOfProjects();
            List<ProjectMember> members = CreateListOfProjectMembers();

            var projectId = 2;

            _mockProjectMemberRepository.Setup(repo => repo.RemoveRangeByProjectIdAsync(projectId)).Callback((int id) => members.RemoveAll(x => x.ProjectId == id));
            _mockProjectRepository.Setup(repo => repo.GetByIdAsync(projectId)).ReturnsAsync(projects.Find(p => p.Id == projectId)!);
            _mockProjectRepository.Setup(repo => repo.RemoveAsync(projectId)).Callback((int id) => projects.RemoveAll(x => x.Id == id));

            await _projectService.DeleteProjectAsync(projectId);
            var isProjectExists = projects.Find(p => p.Id == projectId);
            var isProjectMemberExists = members.Find(m => m.ProjectId == projectId);

            Assert.Null(isProjectExists);
            Assert.Null(isProjectMemberExists);
        }


        [Fact]
        public async void DeleteProjectNotFoundTest()
        {
            List<Project> projects = CreateListOfProjects();

            var projectId = 4;

            _mockProjectRepository.Setup(repo => repo.GetByIdAsync(projectId)).ReturnsAsync(projects.Find(p => p.Id == projectId)!);

            async Task Result() => await _projectService.DeleteProjectAsync(projectId);

            await Assert.ThrowsAsync<ProjectNotFoundException>(Result);
        }


        
        [Fact]
        public async void GetProjectsByUserIdOkTest()
        {
            List<User> users = CreateListOfUsers();
            List<Project> projects = CreateListOfProjects();
            List<ProjectMember> members = CreateListOfProjectMembers();

            var userId = 3;

            _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(users.Find(u => u.Id == userId)!);
            _mockProjectMemberRepository.Setup(repo => repo.GetRangeByUserIdAsync(userId)).ReturnsAsync(members.FindAll(m => m.UserId == userId));
            _mockProjectRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((int id) => projects.Find(p => p.Id == id)!);

            var actualProjects = await _projectService.GetProjectsByUserIdAsync(userId);

            Assert.Equal(1, actualProjects[0].Id);
            Assert.Equal(MemberStatus.Member, actualProjects[0].UserStatus);
            Assert.Equal(3, actualProjects[1].Id);
            Assert.Equal(MemberStatus.Admin, actualProjects[1].UserStatus);
        }


        [Fact]
        public async void GetProjectsByUserIdUserNotFoundTest()
        {
            List<User> users = CreateListOfUsers();
            var userId = 5;

            _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(users.Find(u => u.Id == userId)!);

            async Task Result() => await _projectService.GetProjectsByUserIdAsync(userId);

            await Assert.ThrowsAsync<UserNotFoundException>(Result);
        }



        [Fact]
        public async void GetProjectsByUserIdEmptyTest()
        {
            List<User> users = CreateListOfUsers();
            List<ProjectMember> members = CreateListOfProjectMembers();
            var userId = 3;
            members.RemoveAll(m => m.UserId == userId);

            _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(users.Find(u => u.Id == userId)!);
            _mockProjectMemberRepository.Setup(repo => repo.GetRangeByUserIdAsync(userId)).ReturnsAsync(members.FindAll(m => m.UserId == userId));

            var actualProjects = await _projectService.GetProjectsByUserIdAsync(userId);

            Assert.Empty(actualProjects);
        }


        [Fact]
        public async void GetUsersByProjectIdOkTest()
        {
            List<User> users = CreateListOfUsers();
            List<Project> projects = CreateListOfProjects();
            List<ProjectMember> members = CreateListOfProjectMembers();

            var projectId = 2;

            _mockUserRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((int id) => users.Find(u => u.Id == id)!);
            _mockProjectMemberRepository.Setup(repo => repo.GetRangeByProjectIdAsync(projectId)).ReturnsAsync(members.FindAll(m => m.ProjectId == projectId));
            _mockProjectRepository.Setup(repo => repo.GetByIdAsync(projectId)).ReturnsAsync(projects.Find(p => p.Id == projectId)!);

            var actualUsers = await _projectService.GetUsersByProjectIdAsync(projectId);

            Assert.Equal(1, actualUsers[0].Id);
            Assert.Equal("ladybird", actualUsers[0].Login);
            Assert.Equal(2, actualUsers[1].Id);
            Assert.Equal("cherry", actualUsers[1].Login);
        }


        [Fact]
        public async void GetUsersByProjectIdEmptyTest()
        {
            List<User> users = CreateListOfUsers();
            List<Project> projects = CreateListOfProjects();
            List<ProjectMember> members = CreateListOfProjectMembers();
            var projectId = 2;
            members.RemoveAll(m => m.ProjectId == projectId);

            _mockProjectMemberRepository.Setup(repo => repo.GetRangeByProjectIdAsync(projectId)).ReturnsAsync(members.FindAll(m => m.ProjectId == projectId));
            _mockProjectRepository.Setup(repo => repo.GetByIdAsync(projectId)).ReturnsAsync(projects.Find(p => p.Id == projectId)!);

            var actualUsers = await _projectService.GetUsersByProjectIdAsync(projectId);

            Assert.Empty(actualUsers);
        }


        [Fact]
        public async void GetUsersByProjectIdNotFoundTest()
        {
            List<User> users = CreateListOfUsers();
            List<Project> projects = CreateListOfProjects();
            List<ProjectMember> members = CreateListOfProjectMembers();

            var projectId = 5;

            _mockProjectRepository.Setup(repo => repo.GetByIdAsync(projectId)).ReturnsAsync(projects.Find(p => p.Id == projectId)!);

            async Task Result() => await _projectService.GetUsersByProjectIdAsync(projectId);

            await Assert.ThrowsAsync<ProjectNotFoundException>(Result);
        }



        public List<User> CreateListOfUsers()
        {
            var users = new List<User>()
            {
                new User(1, "ladybird", "redautumn", UserLevel.Free),
                new User(2, "cherry", "blood", UserLevel.Gold),
                new User(3, "nastyrat", "gypsy", UserLevel.Silver),
                new User(4, "anorien", "12345", UserLevel.Gold)
            };

            foreach (var user in users)
            {
                user.SetPassword(user.PasswordHash != null ? user.PasswordHash : "aaaaaaaaa");
                Console.Write(user.PasswordHash);
            }

            return users;
        }


        public List<Project> CreateListOfProjects()
        {
            return new List<Project>()
            {
                new Project(1, "Goblin"),
                new Project(2, "vendetta"),
                new Project(3, "humus")
            };

        }


        public List<ProjectMember> CreateListOfProjectMembers()
        {
            return new List<ProjectMember>()
            {
                new ProjectMember(1, 1, MemberStatus.Admin),
                new ProjectMember(1, 2, MemberStatus.Member),
                new ProjectMember(1, 3, MemberStatus.Member),
                new ProjectMember(2, 2, MemberStatus.Admin),
                new ProjectMember(2, 3, MemberStatus.Member),
                new ProjectMember(3, 1, MemberStatus.Member),
                new ProjectMember(3, 3, MemberStatus.Admin),
            };

        }
    }
}

