using System;
namespace FinanceAcc.Exceptions.ProjectServiceExceptions
{
	public class UnableRemoveAdminException: Exception
	{
        public UnableRemoveAdminException() { }

        public UnableRemoveAdminException(string message)
        : base(message) { }

        public UnableRemoveAdminException(string message, Exception inner)
        : base(message, inner) { }
    }
}

