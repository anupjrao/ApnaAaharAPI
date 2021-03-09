using System;
using System.Collections.Generic;
using System.Text;

namespace ApnaAahar.Exceptions
{
    public class IdenticalPasswordException : ApplicationException
    {
        public IdenticalPasswordException()
        {
        }

        public IdenticalPasswordException(string message) : base(message)
        {
        }

        public IdenticalPasswordException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
