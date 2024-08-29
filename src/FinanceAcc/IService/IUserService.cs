using System;
using FinanceAcc.Models;

namespace FinanceAcc.IService
{
	public interface IUserService
	{
		Task<User> GetUserByLoginAsync(string login);
	}
}

