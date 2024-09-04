using System;
namespace FinanceAcc.Exceptions.ProjectServiceExceptions
{
	public class UserLevelLimitReachedException: Exception
	{
        public UserLevelLimitReachedException() { }

        public UserLevelLimitReachedException(string message)
        : base(message) { }

        public UserLevelLimitReachedException(string message, Exception inner)
        : base(message, inner) { }
    }
}

