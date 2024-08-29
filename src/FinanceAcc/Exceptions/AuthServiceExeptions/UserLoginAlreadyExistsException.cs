using System;
namespace FinanceAcc.Exceptions.AuthServiceExeptions
{
	public class UserLoginAlreadyExistsException: Exception
	{
		public UserLoginAlreadyExistsException() { }

        public UserLoginAlreadyExistsException(string message)
        : base(message) { }

        public UserLoginAlreadyExistsException(string message, Exception inner)
        : base(message, inner) { }
    }
}

