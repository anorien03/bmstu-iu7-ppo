using System;
using FinanceAcc.Exceptions.AuthServiceExeptions;
using FinanceAcc.IRepository;
using FinanceAcc.IService;
using FinanceAcc.Models;

namespace FinanceAcc.Services
{
	public class UserService: IUserService
	{
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository  = userRepository ?? throw new ArgumentNullException();
        }


        public async Task<User> GetUserByLoginAsync(string login)
        {
            var user = await _userRepository.GetUserByLoginAsync(login);

            return user ?? throw new UserLoginNotFoundException();
        }

    }
}

