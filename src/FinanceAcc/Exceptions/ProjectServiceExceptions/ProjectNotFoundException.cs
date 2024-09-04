using System;
namespace FinanceAcc.Exceptions.ProjectServiceExceptions
{
	public class ProjectNotFoundException: Exception
	{
        public ProjectNotFoundException() { }

        public ProjectNotFoundException(string message)
        : base(message) { }

        public ProjectNotFoundException(string message, Exception inner)
        : base(message, inner) { }
    }
}

