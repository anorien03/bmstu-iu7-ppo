using System;
namespace FinanceAcc.Exceptions.ProjectServiceExceptions
{
	public class UserNotInvitedToProjectException: Exception
	{
        public UserNotInvitedToProjectException() { }

        public UserNotInvitedToProjectException(string message)
        : base(message) { }

        public UserNotInvitedToProjectException(string message, Exception inner)
        : base(message, inner) { }
    }
}

