using System;
using System.Web.Helpers;


namespace FinanceAcc.Models
{
	public class User
	{
        public int Id { get; set; }

        public string Login { get; set; }

        public string? PasswordHash { get; private set; }

        public UserLevel Level { get; set; }


		public User(int id, string login, string passwordHash, UserLevel level)
		{
			Id = id;
			Login = login;
            PasswordHash = passwordHash;
			Level = level;
		}

        public User(string login, UserLevel level)
        {
            Login = login;
            PasswordHash = null;
            Level = level;
        }

        public bool VerifyPassword(string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, PasswordHash);
        }

        public void SetPassword(string password)
        {
            //string mySalt = BCrypt.Net.BCrypt.GenerateSalt();
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}

