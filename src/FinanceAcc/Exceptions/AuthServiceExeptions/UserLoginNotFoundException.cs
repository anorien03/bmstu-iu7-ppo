using System;
namespace FinanceAcc.Exceptions.AuthServiceExeptions
{
	public class UserLoginNotFoundException: Exception
	{
		public UserLoginNotFoundException() { }

        public UserLoginNotFoundException(string message)
        : base(message) { }

        public UserLoginNotFoundException(string message, Exception inner)
        : base(message, inner) { }
    }
}

