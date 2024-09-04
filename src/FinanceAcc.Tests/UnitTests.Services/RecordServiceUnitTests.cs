using System;
using System.Linq;
using System.Linq.Expressions;
using FinanceAcc.Exceptions.CategoryServiceException;
using FinanceAcc.Exceptions.ProjectServiceExceptions;
using FinanceAcc.Exceptions.RecordServiceException;
using FinanceAcc.IRepository;
using FinanceAcc.IService;
using FinanceAcc.Models;
using FinanceAcc.Services;
using Moq;
using Record = FinanceAcc.Models.Record;

namespace FinanceAcc.Tests.UnitTests.Services
{
	public class RecordServiceUnitTests
	{
        private readonly IRecordService _recordService;
        private readonly Mock<IRecordRepository> _mockRecordRepository = new();
        private readonly Mock<ICategoryRepository> _mockCategoryRepository = new();
        private readonly Mock<IProjectMemberRepository> _mockProjectMemberRepository = new();

        public RecordServiceUnitTests()
		{
            _recordService = new RecordService(_mockRecordRepository.Object, _mockCategoryRepository.Object, _mockProjectMemberRepository.Object);
		}


        [Fact]
        public async void AddRecordOkTest()
        {
            List<ProjectMember> members = CreateListOfProjectMembers();
            List<Category> categories = CreateListOfCategories();
            List<Record> records = CreateListOfRecords();

            var record = new Record(5, 3, 1, 2, -200, "", new DateTime(2023, 4, 7));

            _mockProjectMemberRepository.Setup(repo => repo.GetByIdAsync(record.UserId, record.ProjectId)).ReturnsAsync(members.Find(m =>
                    m.ProjectId == record.ProjectId && m.UserId == record.UserId)!);
            _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(record.CategoryId)).ReturnsAsync(categories.Find(c =>
                    c.Id == record.CategoryId)!);
            _mockRecordRepository.Setup(repo => repo.AddAsync(record)).Callback((Record r) => records.Add(r));

            await _recordService.AddRecordAsync(record);
            var actualRecord = records.Last();

            Assert.Equal(actualRecord, record);
        }


        [Fact]
        public async void AddRecordUserNotInvitedTest()
        {
            List<ProjectMember> members = CreateListOfProjectMembers();

            var record = new Record(5, 2, 1, 2, -200, "", new DateTime(2023, 4, 7));

            _mockProjectMemberRepository.Setup(repo => repo.GetByIdAsync(record.UserId, record.ProjectId)).ReturnsAsync(members.Find(m =>
                    m.ProjectId == record.ProjectId && m.UserId == record.UserId)!);

            async Task result() => await _recordService.AddRecordAsync(record);

            await Assert.ThrowsAsync<UserNotInvitedToProjectException>(result);
        }


        [Fact]
        public async void AddRecordCategoryNotFoundTest()
        {
            List<ProjectMember> members = CreateListOfProjectMembers();
            List<Category> categories = CreateListOfCategories();

            var record = new Record(5, 3, 1, 7, -200, "", new DateTime(2023, 4, 7));

            _mockProjectMemberRepository.Setup(repo => repo.GetByIdAsync(record.UserId, record.ProjectId)).ReturnsAsync(members.Find(m =>
                    m.ProjectId == record.ProjectId && m.UserId == record.UserId)!);
            _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(record.CategoryId)).ReturnsAsync(categories.Find(c =>
                    c.Id == record.CategoryId)!);

            async Task result() => await _recordService.AddRecordAsync(record);

            await Assert.ThrowsAsync<CategoryNotFoundException>(result);
        }


        [Fact]
        public async void GetRecordsByProjectIdOkTest()
        {
            List<ProjectMember> members = CreateListOfProjectMembers();
            List<Category> categories = CreateListOfCategories();
            List<Record> records = CreateListOfRecords();

            var projectId = 1;
            int? categoryId = null;
            Expression<Func<Record, bool>> expression = x => categoryId == null ? x.ProjectId == projectId : x.CategoryId == categoryId;


            _mockProjectMemberRepository.Setup(repo => repo.GetRangeByProjectIdAsync(projectId)).ReturnsAsync(members.FindAll(m =>
                    m.ProjectId == projectId));
            _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(categoryId ?? 1)).ReturnsAsync(categories.Find(c =>
                    c.Id == categoryId)!);
            _mockRecordRepository.Setup(repo => repo.GetFilteredAsync(x =>
                    categoryId == null ? x.ProjectId == projectId : x.CategoryId == categoryId))
                    .ReturnsAsync(records.Where(t => expression.Compile().Invoke(t)).ToList());

            var actualRecords = await _recordService.GetRecordsAsync(projectId, categoryId);
            var expectedRecords = records;

            Assert.Equal(expectedRecords, actualRecords);
        }


        [Fact]
        public async void GetRecordsByCategoryIdOkTest()
        {
            List<ProjectMember> members = CreateListOfProjectMembers();
            List<Category> categories = CreateListOfCategories();
            List<Record> records = CreateListOfRecords();

            var projectId = 1;
            int? categoryId = 3;
            Expression<Func<Record, bool>> expression = x => categoryId == null ? x.ProjectId == projectId : x.CategoryId == categoryId;


            _mockProjectMemberRepository.Setup(repo => repo.GetRangeByProjectIdAsync(projectId)).ReturnsAsync(members.FindAll(m =>
                    m.ProjectId == projectId));
            _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(categoryId ?? 1)).ReturnsAsync(categories.Find(c =>
                    c.Id == categoryId)!);
            _mockRecordRepository.Setup(repo => repo.GetFilteredAsync(x =>
                    categoryId == null ? x.ProjectId == projectId : x.CategoryId == categoryId))
                    .ReturnsAsync(records.Where(t => expression.Compile().Invoke(t)).ToList());

            var actualRecords = await _recordService.GetRecordsAsync(projectId, categoryId);
            var expectedRecords = records.GetRange(0, 1);

            Assert.Equal(expectedRecords, actualRecords);
        }


        [Fact]
        public async void GetRecordsProjectNotFoundTest()
        {
            List<ProjectMember> members = CreateListOfProjectMembers();

            var projectId = 3;
            

            _mockProjectMemberRepository.Setup(repo => repo.GetRangeByProjectIdAsync(projectId)).ReturnsAsync(members.FindAll(m =>
                    m.ProjectId == projectId));

            async Task result() => await _recordService.GetRecordsAsync(projectId, null);

            await Assert.ThrowsAsync<ProjectNotFoundException>(result);
        }


        [Fact]
        public async void GetRecordsCategoryNotFoundTest()
        {
            List<ProjectMember> members = CreateListOfProjectMembers();
            List<Category> categories = CreateListOfCategories();

            var projectId = 1;
            int? categoryId = 7;


            _mockProjectMemberRepository.Setup(repo => repo.GetRangeByProjectIdAsync(projectId)).ReturnsAsync(members.FindAll(m =>
                    m.ProjectId == projectId));
            _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(categoryId ?? 1)).ReturnsAsync(categories.Find(c =>
                    c.Id == categoryId)!);

            async Task result() => await _recordService.GetRecordsAsync(projectId, categoryId);

            await Assert.ThrowsAsync<CategoryNotFoundException>(result);
        }


        [Fact]
        public async void RemoveRecordOkTest()
        {
            List<Record> records = CreateListOfRecords();

            var recordId = 4;

            _mockRecordRepository.Setup(repo => repo.GetByIdAsync(recordId)).ReturnsAsync(records.Find(c =>
                    c.Id == recordId)!);
            _mockRecordRepository.Setup(repo => repo.RemoveAsync(recordId)).Callback((int recordId) =>
                    records.RemoveAll(r => r.Id == recordId));

            await _recordService.RemoveRecordAsync(recordId);
            var doesRecordExist = records.Find(r => r.Id == recordId);

            Assert.Null(doesRecordExist);
        }


        [Fact]
        public async void RemoveRecordNotFoundTest()
        {
            List<Record> records = CreateListOfRecords();

            var recordId = 10;

            _mockRecordRepository.Setup(repo => repo.GetByIdAsync(recordId)).ReturnsAsync(records.Find(c =>
                    c.Id == recordId)!);

            async Task result() => await _recordService.RemoveRecordAsync(recordId);

            await Assert.ThrowsAsync<RecordNotFoundException>(result);
        }



        public List<ProjectMember> CreateListOfProjectMembers()
        {
            return new List<ProjectMember>()
            {
                new ProjectMember(1, 1, MemberStatus.Admin),
                new ProjectMember(3, 1, MemberStatus.Member),
                new ProjectMember(1, 2, MemberStatus.Member),
                new ProjectMember(2, 2, MemberStatus.Admin),
                new ProjectMember(3, 2, MemberStatus.Invited)
            };

        }


        public List<Category> CreateListOfCategories()
        {
            return new List<Category>()
            {
                new Category(1, 1, "Food"),
                new Category(2, 1, "Apartment"),
                new Category(3, 1, "Education"),
                new Category(4, 1, "Travel"),
                new Category(5, 1, "Leisure"),
                new Category(6, 2, "Food"),
                new Category(7, 2, "Apartment"),
                new Category(8, 2, "Education"),
                new Category(9, 2, "Travel"),
                new Category(10, 2, "Leisure")
            };

        }


        public List<Record> CreateListOfRecords()
        {
            return new List<Record>()
            {
                new Record(1, 1, 1, 3, -500, "Textbook", DateTime.Today.AddDays(-150)),
                new Record(2, 3, 1, 1, -500, null, DateTime.Today.AddDays(-200)),
                new Record(3, 3, 1, 4, -1000, "Bus", DateTime.Today.AddDays(-380)),
                new Record(4, 1, 1, 5, -1000, null, DateTime.Today.AddDays(-400)),
            };

        }
    }
}

