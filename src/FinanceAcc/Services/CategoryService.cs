using System;
using FinanceAcc.Exceptions.ProjectServiceExceptions;
using FinanceAcc.IRepository;
using FinanceAcc.IService;
using FinanceAcc.Models;

namespace FinanceAcc.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProjectRepository _projectRepository;

        private  List<Category> basicCategories = new List<Category>{
            new Category(0,"Food"),
            new Category(0,"Apartment"),
            new Category(0,"Education"),
            new Category(0,"Travel"),
            new Category(0,"Leisure"),
        };

        public CategoryService(ICategoryRepository categoryRepository, IProjectRepository projectRepository)
		{
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException();
            _projectRepository = projectRepository ?? throw new ArgumentNullException();

        }

        public async Task<List<Category>> GetCategoriesByProjectIdAsync(int projectId)
        {
            if (await _projectRepository.GetByIdAsync(projectId) == null)
            {
                throw new ProjectNotFoundException($"Project with id {projectId} not found.");
            }

            return await _categoryRepository.GetRangeByProjectIdAsync(projectId);
        }


        public async Task AddCategoriesToProjectAsync(int projectId)
        {
            if (await _projectRepository.GetByIdAsync(projectId) == null)
            {
                throw new ProjectNotFoundException($"Project with id {projectId} not found.");
            }

            basicCategories.ForEach(c => c.ProjectId = projectId);
            await _categoryRepository.AddRangeAsync(basicCategories);
        }
    }
}

