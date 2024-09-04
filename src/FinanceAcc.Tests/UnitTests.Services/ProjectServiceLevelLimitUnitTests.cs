using System;
using FinanceAcc.Exceptions.ProjectServiceExceptions;
using FinanceAcc.IRepository;
using FinanceAcc.IService;
using FinanceAcc.Models;
using FinanceAcc.Services;
using Moq;

namespace FinanceAcc.Tests.UnitTests.Services
{
    public class ProjectServiceLevelLimitUnitTests
    {
        private readonly IProjectService _projectService;
        private readonly Mock<IUserRepository> _mockUserRepository = new();
        private readonly Mock<IProjectRepository> _mockProjectRepository = new();
        private readonly Mock<IProjectMemberRepository> _mockProjectMemberRepository = new();

        public ProjectServiceLevelLimitUnitTests()
        {
            _projectService = new ProjectService(_mockProjectRepository.Object, _mockProjectMemberRepository.Object, _mockUserRepository.Object);
        }


        [Fact]
        public async void CreateProjectFreeLevelOkTest()
        {
            List<User> users = CreateListOfUsers();
            List<Project> projects = CreateListOfProjects();
            List<ProjectMember> members = CreateListOfProjectMembers();

            var user = users[0];
            var newProject = new Project(30, "King");

            _mockProjectMemberRepository.Setup(repo => repo.CountProjectsAsync(user.Id, MemberStatus.Admin)).ReturnsAsync(members.Count(u =>
                    u.UserId == user.Id & u.Status == MemberStatus.Admin));
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
        public async void CreateProjectFreeLevelLimitReachedTest()
        {
            List<User> users = CreateListOfUsers();
            List<Project> projects = CreateListOfProjects();
            List<ProjectMember> members = CreateListOfProjectMembers();

            var user = users[0];
            members.Add(new ProjectMember(user.Id, 31, MemberStatus.Admin));
            var newProject = new Project(30, "King");

            _mockProjectMemberRepository.Setup(repo => repo.CountProjectsAsync(user.Id, MemberStatus.Admin)).ReturnsAsync(members.Count(u =>
                    u.UserId == user.Id & u.Status == MemberStatus.Admin));

            async Task Result() => await _projectService.CreateProjectAsync(user, newProject);

            await Assert.ThrowsAsync<UserLevelLimitReachedException>(Result);
        }


        [Fact]
        public async void CreateProjectSilverLevelOkTest()
        {
            List<User> users = CreateListOfUsers();
            List<Project> projects = CreateListOfProjects();
            List<ProjectMember> members = CreateListOfProjectMembers();

            var user = users[1];
            var newProject = new Project(30, "King");

            _mockProjectMemberRepository.Setup(repo => repo.CountProjectsAsync(user.Id, MemberStatus.Admin)).ReturnsAsync(members.Count(u =>
                    u.UserId == user.Id & u.Status == MemberStatus.Admin));
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
        public async void CreateProjectSilverLevelLimitReachedTest()
        {
            List<User> users = CreateListOfUsers();
            List<Project> projects = CreateListOfProjects();
            List<ProjectMember> members = CreateListOfProjectMembers();

            var user = users[1];
            members.Add(new ProjectMember(user.Id, 31, MemberStatus.Admin));
            var newProject = new Project(30, "King");

            _mockProjectMemberRepository.Setup(repo => repo.CountProjectsAsync(user.Id, MemberStatus.Admin)).ReturnsAsync(members.Count(u =>
                    u.UserId == user.Id & u.Status == MemberStatus.Admin));

            async Task Result() => await _projectService.CreateProjectAsync(user, newProject);

            await Assert.ThrowsAsync<UserLevelLimitReachedException>(Result);
        }


        [Fact]
        public async void CreateProjectGoldLevelOkTest()
        {
            List<User> users = CreateListOfUsers();
            List<Project> projects = CreateListOfProjects();
            List<ProjectMember> members = CreateListOfProjectMembers();

            var user = users[2];
            members.Add(new ProjectMember(user.Id, 31, MemberStatus.Admin));
            var newProject = new Project(30, "King");

            _mockProjectMemberRepository.Setup(repo => repo.CountProjectsAsync(user.Id, MemberStatus.Admin)).ReturnsAsync(members.Count(u =>
                    u.UserId == user.Id & u.Status == MemberStatus.Admin));
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
        public async void AcceptInvitingFreeLevelOkTest()
        {
            List<User> users = CreateListOfUsers();
            List<ProjectMember> members = CreateListOfProjectMembers();

            var user = users[0];
            var projectId = 4;

            _mockProjectMemberRepository.Setup(repo => repo.CountProjectsAsync(user.Id, MemberStatus.Member)).ReturnsAsync(members.Count(u =>
                    u.UserId == user.Id && u.Status == MemberStatus.Member));
            _mockProjectMemberRepository.Setup(repo => repo.GetByIdAsync(user.Id, projectId)).ReturnsAsync(members.Find(m =>
                    m.ProjectId == projectId && m.UserId == user.Id)!);
            _mockProjectMemberRepository.Setup(repo => repo.UpdateAsync(It.IsAny<ProjectMember>())).Callback((ProjectMember member) =>
                    (members.Find(m => m.UserId == member.UserId && m.ProjectId == member.ProjectId) ?? members.First()).Status = member.Status);

            await _projectService.AcceptInvitingAsync(user, projectId);
            var actualMember = members.Find(m => m.UserId == user.Id && m.ProjectId == projectId) ?? members.First();

            Assert.Equal(MemberStatus.Member, actualMember.Status);
        }


        [Fact]
        public async void AcceptInvitingFreeLevelLimitReachedTest()
        {
            List<User> users = CreateListOfUsers();
            List<ProjectMember> members = CreateListOfProjectMembers();

            var user = users[0];
            members[2].Status = MemberStatus.Member;
            var projectId = 5;

            _mockProjectMemberRepository.Setup(repo => repo.CountProjectsAsync(user.Id, MemberStatus.Member)).ReturnsAsync(members.Count(u =>
                    u.UserId == user.Id && u.Status == MemberStatus.Member));
            _mockProjectMemberRepository.Setup(repo => repo.GetByIdAsync(user.Id, projectId)).ReturnsAsync(members.Find(m =>
                    m.ProjectId == projectId && m.UserId == user.Id)!);
            _mockProjectMemberRepository.Setup(repo => repo.UpdateAsync(It.IsAny<ProjectMember>())).Callback((ProjectMember member) =>
                    (members.Find(m => m.UserId == member.UserId && m.ProjectId == member.ProjectId) ?? members.First()).Status = member.Status);

            async Task Result() => await _projectService.AcceptInvitingAsync(user, projectId);

            await Assert.ThrowsAsync<UserLevelLimitReachedException>(Result);
        }


        [Fact]
        public async void AcceptInvitingSilverLevelOkTest()
        {
            List<User> users = CreateListOfUsers();
            List<ProjectMember> members = CreateListOfProjectMembers();

            var user = users[1];
            var projectId = 10;

            _mockProjectMemberRepository.Setup(repo => repo.CountProjectsAsync(user.Id, MemberStatus.Member)).ReturnsAsync(members.Count(u =>
                    u.UserId == user.Id && u.Status == MemberStatus.Member));
            _mockProjectMemberRepository.Setup(repo => repo.GetByIdAsync(user.Id, projectId)).ReturnsAsync(members.Find(m =>
                    m.ProjectId == projectId && m.UserId == user.Id)!);
            _mockProjectMemberRepository.Setup(repo => repo.UpdateAsync(It.IsAny<ProjectMember>())).Callback((ProjectMember member) =>
                    (members.Find(m => m.UserId == member.UserId && m.ProjectId == member.ProjectId) ?? members.First()).Status = member.Status);

            await _projectService.AcceptInvitingAsync(user, projectId);
            var actualMember = members.Find(m => m.UserId == user.Id && m.ProjectId == projectId) ?? members.First();

            Assert.Equal(MemberStatus.Member, actualMember.Status);
        }


        [Fact]
        public async void AcceptInvitingSilverLevelLimitReachedTest()
        {
            List<User> users = CreateListOfUsers();
            List<ProjectMember> members = CreateListOfProjectMembers();

            var user = users[1];
            (members.Find(m => m.ProjectId == 10 && m.UserId == user.Id) ?? members.First()).Status = MemberStatus.Member;
            var projectId = 11;

            _mockProjectMemberRepository.Setup(repo => repo.CountProjectsAsync(user.Id, MemberStatus.Member)).ReturnsAsync(members.Count(u =>
                    u.UserId == user.Id && u.Status == MemberStatus.Member));
            _mockProjectMemberRepository.Setup(repo => repo.GetByIdAsync(user.Id, projectId)).ReturnsAsync(members.Find(m =>
                    m.ProjectId == projectId && m.UserId == user.Id)!);
            _mockProjectMemberRepository.Setup(repo => repo.UpdateAsync(It.IsAny<ProjectMember>())).Callback((ProjectMember member) =>
                    (members.Find(m => m.UserId == member.UserId && m.ProjectId == member.ProjectId) ?? members.First()).Status = member.Status);

            async Task Result() => await _projectService.AcceptInvitingAsync(user, projectId);

            await Assert.ThrowsAsync<UserLevelLimitReachedException>(Result);
        }


        [Fact]
        public async void AcceptInvitingGoldLevelOkTest()
        {
            List<User> users = CreateListOfUsers();
            List<ProjectMember> members = CreateListOfProjectMembers();

            var user = users[2];
            (members.Find(m => m.ProjectId == 20 && m.UserId == user.Id) ?? members.First()).Status = MemberStatus.Member;
            var projectId = 21;

            _mockProjectMemberRepository.Setup(repo => repo.CountProjectsAsync(user.Id, MemberStatus.Member)).ReturnsAsync(members.Count(u =>
                    u.UserId == user.Id && u.Status == MemberStatus.Member));
            _mockProjectMemberRepository.Setup(repo => repo.GetByIdAsync(user.Id, projectId)).ReturnsAsync(members.Find(m =>
                    m.ProjectId == projectId && m.UserId == user.Id)!);
            _mockProjectMemberRepository.Setup(repo => repo.UpdateAsync(It.IsAny<ProjectMember>())).Callback((ProjectMember member) =>
                    (members.Find(m => m.UserId == member.UserId && m.ProjectId == member.ProjectId) ?? members.First()).Status = member.Status);

            await _projectService.AcceptInvitingAsync(user, projectId);
            var actualMember = members.Find(m => m.UserId == user.Id && m.ProjectId == projectId) ?? members.First();

            Assert.Equal(MemberStatus.Member, actualMember.Status);
        }


        public List<User> CreateListOfUsers()
        {
            return new List<User>()
            {
                new User(1, "ladybird", "redautumn", UserLevel.Free),
                new User(2, "nastyrat", "gypsy", UserLevel.Silver),
                new User(3, "cherry", "blood", UserLevel.Gold),
            };
        }


        public List<Project> CreateListOfProjects()
        {
            return new List<Project>()
            {
                new Project(1, "Goblin"),
                new Project(2, "vendetta"),
                new Project(3, "humus"),
                new Project(4, "victor"),
                new Project(5, "hulio1"),
                new Project(6, "hulio2"),
                new Project(7, "hulio3"),
                new Project(8, "hulio4"),
                new Project(9, "hulio5"),
                new Project(10, "hulio6"),
                new Project(11, "hulio7"),
                new Project(12, "hulio8"),
                new Project(13, "hulio9"),
                new Project(14, "hulio10"),
                new Project(15, "hulio11"),
                new Project(16, "hulio12"),
                new Project(17, "hulio13"),
                new Project(18, "hulio14"),
                new Project(19, "hulio15"),
                new Project(20, "hulio16"),
                new Project(21, "hulio17"),
                new Project(31, "hulio18")
            };

        }


        public List<ProjectMember> CreateListOfProjectMembers()
        {
            return new List<ProjectMember>()
            {
                new ProjectMember(1, 1, MemberStatus.Member),
                new ProjectMember(1, 2, MemberStatus.Member),
                new ProjectMember(1, 4, MemberStatus.Invited),
                new ProjectMember(1, 5, MemberStatus.Invited),

                new ProjectMember(2, 1, MemberStatus.Admin),
                new ProjectMember(2, 2, MemberStatus.Admin),
                new ProjectMember(2, 3, MemberStatus.Member),
                new ProjectMember(2, 4, MemberStatus.Member),
                new ProjectMember(2, 5, MemberStatus.Member),
                new ProjectMember(2, 6, MemberStatus.Member),
                new ProjectMember(2, 7, MemberStatus.Member),
                new ProjectMember(2, 10, MemberStatus.Invited),
                new ProjectMember(2, 11, MemberStatus.Invited),

                new ProjectMember(3, 3, MemberStatus.Admin),
                new ProjectMember(3, 4, MemberStatus.Admin),
                new ProjectMember(3, 5, MemberStatus.Admin),
                new ProjectMember(3, 6, MemberStatus.Admin),
                new ProjectMember(3, 7, MemberStatus.Admin),
                new ProjectMember(3, 8, MemberStatus.Admin),
                new ProjectMember(3, 9, MemberStatus.Admin),
                new ProjectMember(3, 1, MemberStatus.Member),
                new ProjectMember(3, 2, MemberStatus.Member),
                new ProjectMember(3, 10, MemberStatus.Member),
                new ProjectMember(3, 11, MemberStatus.Member),
                new ProjectMember(3, 12, MemberStatus.Member),
                new ProjectMember(3, 13, MemberStatus.Member),
                new ProjectMember(3, 14, MemberStatus.Member),
                new ProjectMember(3, 15, MemberStatus.Member),
                new ProjectMember(3, 16, MemberStatus.Member),
                new ProjectMember(3, 17, MemberStatus.Member),
                new ProjectMember(3, 18, MemberStatus.Member),
                new ProjectMember(3, 19, MemberStatus.Member),
                new ProjectMember(3, 20, MemberStatus.Invited),
                new ProjectMember(3, 21, MemberStatus.Invited)
            };

        }
    }
}

