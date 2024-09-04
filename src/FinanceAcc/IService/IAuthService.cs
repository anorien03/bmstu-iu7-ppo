using System;
using FinanceAcc.Models;

namespace FinanceAcc.IService
{
	public interface IAuthService
	{
		Task<User> Login(string login, string password);

		Task Register(User user, string password);
	}
}

