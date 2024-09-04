using System;
namespace FinanceAcc.Exceptions.RecordServiceException
{
	public class InvalidDateException: Exception
	{
        public InvalidDateException() { }

        public InvalidDateException(string message)
        : base(message) { }

        public InvalidDateException(string message, Exception inner)
        : base(message, inner) { }
    }
}

