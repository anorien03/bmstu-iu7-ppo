using System;
using FinanceAcc.Exceptions.ProjectServiceExceptions;
using FinanceAcc.IRepository;
using FinanceAcc.IService;
using FinanceAcc.Models;
using FinanceAcc.Services;
using Moq;

namespace FinanceAcc.Tests.UnitTests.Services
{
	public class CategoryServiceUnitTests
	{
        private readonly ICategoryService _categoryService;
        private readonly Mock<IProjectRepository> _mockProjectRepository = new();
        private readonly Mock<ICategoryRepository> _mockCategoryRepository = new();

        public CategoryServiceUnitTests()
		{
			_categoryService = new CategoryService(_mockCategoryRepository.Object, _mockProjectRepository.Object);
		}


        [Fact]
        public async void GetCategoriesByProjectIdOkTest()
        {
            List<Project> projects = CreateListOfProjects();
            List<Category> categories = CreateListOfCategories();

            var projectId = 2;

            _mockCategoryRepository.Setup(repo => repo.GetRangeByProjectIdAsync(projectId)).ReturnsAsync(categories.FindAll(u => u.ProjectId == projectId)!);
            _mockProjectRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((int id) => projects.Find(p => p.Id == id)!);

            var actualCategories = await _categoryService.GetCategoriesByProjectIdAsync(projectId);
            var expectedCategories = categories.GetRange(5, 5);

            Assert.Equal(expectedCategories, actualCategories);
        }


        [Fact]
        public async void GetCategoriesByProjectIdNotFoundTest()
        {
            List<Project> projects = CreateListOfProjects();

            var projectId = 4;

            _mockProjectRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((int id) => projects.Find(p => p.Id == id)!);

            async Task Result() => await _categoryService.GetCategoriesByProjectIdAsync(projectId);

            await Assert.ThrowsAsync<ProjectNotFoundException>(Result);
        }


        [Fact]
        public async void GetCategoriesByProjectEmptyTest()
        {
            List<Project> projects = CreateListOfProjects();
            List<Category> categories = CreateListOfCategories();

            var projectId = 3;

            _mockCategoryRepository.Setup(repo => repo.GetRangeByProjectIdAsync(projectId)).ReturnsAsync(categories.FindAll(u => u.ProjectId == projectId)!);
            _mockProjectRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((int id) => projects.Find(p => p.Id == id)!);

            var actualCategories = await _categoryService.GetCategoriesByProjectIdAsync(projectId);

            Assert.Empty(actualCategories);
        }


        [Fact]
        public async void AddCategoriesToProjectOkTest()
        {
            List<Project> projects = CreateListOfProjects();
            List<Category> categories = CreateListOfCategories();

            var projectId = 3;

            _mockCategoryRepository.Setup(repo => repo.AddRangeAsync(It.IsAny<List<Category>>())).Callback((List<Category> c) => categories.AddRange(c));
            _mockProjectRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((int id) => projects.Find(p => p.Id == id)!);

            await _categoryService.AddCategoriesToProjectAsync(projectId);
            var actualCategories = categories.FindAll(c => c.ProjectId == projectId);

            Assert.Equal(5, actualCategories.Count);
        }


        [Fact]
        public async void AddCategoriesToProjectNotFoundTest()
        {
            List<Project> projects = CreateListOfProjects();

            var projectId = 4;

            _mockProjectRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((int id) => projects.Find(p => p.Id == id)!);

            async Task Result() => await _categoryService.GetCategoriesByProjectIdAsync(projectId);

            await Assert.ThrowsAsync<ProjectNotFoundException>(Result);
        }


        public List<Project> CreateListOfProjects()
        {
            return new List<Project>()
            {
                new Project(1, "Goblin"),
                new Project(2, "Flower"),
                new Project(3, "China")
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
    }
}

