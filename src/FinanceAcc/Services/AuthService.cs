using System;
using FinanceAcc.Exceptions.AuthServiceExeptions;
using FinanceAcc.IRepository;
using FinanceAcc.IService;
using FinanceAcc.Models;

namespace FinanceAcc.Services
{
	public class AuthService: IAuthService
	{
        private readonly IUserRepository _userRepository;

		public AuthService(IUserRepository userRepository)
		{
            _userRepository = userRepository ?? throw new ArgumentNullException();
		}


        public async Task<User> Login(string login, string password)
        {
            var user = await _userRepository.GetUserByLoginAsync(login);

            if (user == null)
            {
                throw new UserLoginNotFoundException($"User with login {login} not found");
            }

            if (!user.VerifyPassword(password))
            {
                throw new IncorrectPasswordException("Incorrect password");
            }

            return user;
        }


        public async Task Register(User user, string password)
        {
            if (await _userRepository.GetUserByLoginAsync(user.Login) != null)
            {
                throw new UserLoginAlreadyExistsException($"User with login {user.Login} already exists");
            }

            user.SetPassword(password);
            await _userRepository.AddAsync(user);

        }
    }
}

