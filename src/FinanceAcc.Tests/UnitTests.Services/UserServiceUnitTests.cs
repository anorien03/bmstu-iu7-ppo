using System;
using FinanceAcc.Exceptions.AuthServiceExeptions;
using FinanceAcc.IRepository;
using FinanceAcc.IService;
using FinanceAcc.Models;
using FinanceAcc.Services;
using Moq;
using Xunit;

namespace FinanceAcc.Tests.UnitTests.Services
{
	public class UserServiceUnitTests
	{
        private readonly IUserService _userService;
        private readonly Mock<IUserRepository> _mockUserRepository = new();

        public UserServiceUnitTests()
		{
			_userService = new UserService(_mockUserRepository.Object);
		}


		[Fact]
		public async void GetUserByLoginNotFoundTest()
		{
            var users = CreateListOfUsers();
            var login = "blueberry";

            _mockUserRepository.Setup(repo => repo.GetUserByLoginAsync(login)).ReturnsAsync(users.Find(u => u.Login == login)!);

            async Task<User> Result() => await _userService.GetUserByLoginAsync(login);

            await Assert.ThrowsAsync<UserLoginNotFoundException>(Result);
		}


        [Fact]
        public async void GetUserByLoginOkTest()
        {
            var users = CreateListOfUsers();
            var login = "cherry";
            var expectedUser = users[1];

            _mockUserRepository.Setup(repo => repo.GetUserByLoginAsync(login)).ReturnsAsync(users.Find(u => u.Login == login)!);

            var returnedUser = await _userService.GetUserByLoginAsync(login);

            Assert.Equal(expectedUser, returnedUser);
        }


        public List<User> CreateListOfUsers()
        {
            var users = new List<User>()
            {
                new User(1, "ladybird", "redautumn", UserLevel.Free),
                new User(2, "cherry", "blood", UserLevel.Gold),
                new User(3, "nastyrat", "gypsy", UserLevel.Silver),
                new User(4, "anorien", "12345", UserLevel.Gold)
            };

            foreach (var user in users)
            {
                user.SetPassword(user.PasswordHash != null ? user.PasswordHash : "aaaaaaaaa");
                Console.Write(user.PasswordHash);
            }

            return users;
        }
    }
}

