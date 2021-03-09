using System;
using System.Collections.Generic;
using System.Text;

namespace ApnaAahar.Exceptions
{
    public class DbContextException : ApplicationException
    {
        public DbContextException()
        {
        }

        public DbContextException(string message) : base(message)
        {
        }

        public DbContextException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
