using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using FinanceAcc.Exceptions.AuthServiceExeptions;
using FinanceAcc.Exceptions.CategoryServiceException;
using FinanceAcc.Exceptions.ProjectServiceExceptions;
using FinanceAcc.Exceptions.RecordServiceException;
using FinanceAcc.IRepository;
using FinanceAcc.IService;
using FinanceAcc.Models;

namespace FinanceAcc.Services
{
	public class RecordService: IRecordService
	{
        private readonly IRecordRepository _recordRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProjectMemberRepository _projectMemberRepository;

        public RecordService(IRecordRepository recordRepository, ICategoryRepository categoryRepository, IProjectMemberRepository projectMemberRepository)
		{
            _recordRepository = recordRepository ?? throw new ArgumentNullException();
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException();
            _projectMemberRepository = projectMemberRepository ?? throw new ArgumentNullException();
        }

        public async Task AddRecordAsync(Record record)
        {
            if (await _projectMemberRepository.GetByIdAsync(record.UserId, record.ProjectId) == null)
            {
                throw new UserNotInvitedToProjectException("User cannot create record in project");
            }
            var category = await _categoryRepository.GetByIdAsync(record.CategoryId);
            if (category == null || category.ProjectId != record.ProjectId)
            {
                throw new CategoryNotFoundException("Category not found in project");
            }

            await _recordRepository.AddAsync(record);
        }


        public async Task<List<Record>> GetRecordsAsync(int projectId, int? categoryId)
        {
            if ((await _projectMemberRepository.GetRangeByProjectIdAsync(projectId)).Count == 0)
            {
                throw new ProjectNotFoundException("Project not found");
            }
            if (categoryId != null)
            {
                var category = await _categoryRepository.GetByIdAsync(categoryId ?? 1);
                if (category == null || category.ProjectId != projectId)
                {
                    throw new CategoryNotFoundException("Category not found in project");
                }
            }

            return await _recordRepository.GetFilteredAsync(x =>
                    categoryId == null ? x.ProjectId == projectId : x.CategoryId == categoryId);
        }


        public async Task RemoveRecordAsync(int recordId)
        {
            if (await _recordRepository.GetByIdAsync(recordId) == null)
            {
                throw new RecordNotFoundException($"Record with id {recordId} not found");
            }

            await _recordRepository.RemoveAsync(recordId);
        }
    }
}

