using System;
namespace FinanceAcc.Exceptions.ProjectServiceExceptions
{
	public class UserAlreadyInvitedException: Exception
	{
        public UserAlreadyInvitedException() { }

        public UserAlreadyInvitedException(string message)
        : base(message) { }

        public UserAlreadyInvitedException(string message, Exception inner)
        : base(message, inner) { }
    }
}

