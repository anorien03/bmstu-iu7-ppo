using System;
using System.Runtime.ConstrainedExecution;
using FinanceAcc.Exceptions.AuthServiceExeptions;
using FinanceAcc.IRepository;
using FinanceAcc.IService;
using FinanceAcc.Models;
using FinanceAcc.Services;
using Moq;

namespace FinanceAcc.Tests.UnitTests.Services
{
	public class AuthServiceUnitTests
	{
		private readonly IAuthService _authService;
		private readonly Mock<IUserRepository> _mockUserRepository = new();

		public AuthServiceUnitTests()
		{
			_authService = new AuthService(_mockUserRepository.Object);
		}


		[Fact]
		public async void LoginOkTest()
		{
			var login = "anorien";
			var password = "12345";
			List<User> users = CreateListOfUsers();
			var expectedUser = users.Last();
			_mockUserRepository.Setup(repo => repo.GetUserByLoginAsync(login)).ReturnsAsync(users.Find(user => user.Login == login)!);

			var user = await _authService.Login(login, password);

			Assert.Equal(user, expectedUser);

		}

		[Fact]
		public async void LoginIncorrectPasswordTest()
		{
			var login = "anorien";
			var password = "kukareku";
			List<User> users = CreateListOfUsers();
			_mockUserRepository.Setup(repo => repo.GetUserByLoginAsync(login)).ReturnsAsync(users.Find(user => user.Login == login)!);

			async Task<User> Result() => await _authService.Login(login, password);

			await Assert.ThrowsAsync<IncorrectPasswordException>(Result);

		}

		[Fact]
		public async void LoginNotFoundTest()
		{
			var login = "qwerty";
			var password = "kukareku";
			List<User> users = CreateListOfUsers();
			_mockUserRepository.Setup(repo => repo.GetUserByLoginAsync(login)).ReturnsAsync(users.Find(user => user.Login == login)!);

			async Task<User> Result() => await _authService.Login(login, password);

			await Assert.ThrowsAsync<UserLoginNotFoundException>(Result);

		}


		[Fact]
		public async void RegisterLoginAlreadyExistsTest()
		{
			var user = new User("anorien", UserLevel.Free);
			var password = "kukareku";
			List<User> users = CreateListOfUsers();
			_mockUserRepository.Setup(repo => repo.GetUserByLoginAsync(user.Login)).ReturnsAsync(users.Find(u => u.Login == user.Login)!);

			async Task Result() => await _authService.Register(user, password);

			await Assert.ThrowsAsync<UserLoginAlreadyExistsException>(Result);
		}


		[Fact]
		public async void RegisterOkTest()
		{
			var user = new User("Valdemar", UserLevel.Free);
			var password = "kukareku";
			List<User> users = CreateListOfUsers();

			_mockUserRepository.Setup(repo => repo.GetUserByLoginAsync(user.Login)).ReturnsAsync(users.Find(u => u.Login == user.Login)!);
			_mockUserRepository.Setup(repo => repo.AddAsync(user)).Callback((User u) => users.Add(user));

			await _authService.Register(user, password);
			var actualUser = users.Last();

            Assert.Equal(user, actualUser);
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

