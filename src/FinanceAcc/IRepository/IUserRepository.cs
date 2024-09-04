using System;
using System.Threading.Tasks;
using FinanceAcc.Models;

namespace FinanceAcc.IRepository
{
	public interface IUserRepository: IBaseRepository<User>
    {
        Task<User> GetByLoginAsync(string login);

        Task<User> GetByIdAsync(int id);
    }
}

