using System;
namespace FinanceAcc.Exceptions.ProjectServiceExceptions
{
	public class UserHasAlreadyJoinedException: Exception
	{
        public UserHasAlreadyJoinedException() { }

        public UserHasAlreadyJoinedException(string message)
        : base(message) { }

        public UserHasAlreadyJoinedException(string message, Exception inner)
        : base(message, inner) { }
    }
}

