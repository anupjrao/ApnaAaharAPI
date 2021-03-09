using System;
using System.Collections.Generic;
using System.Text;

namespace ApnaAahar.Exceptions
{
   public class AnySqlException : Exception
    {
        public AnySqlException() { }
        public AnySqlException(string message) : base(message) { }
        public AnySqlException(string message, Exception inner) : base(message, inner) { }
    }
}
