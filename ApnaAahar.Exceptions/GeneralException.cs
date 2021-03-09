using System;
using System.Collections.Generic;
using System.Text;

namespace ApnaAahar.Exceptions
{
    public class GeneralException : Exception
    {
        public GeneralException() { }
        public GeneralException(string message) : base(message) { }
        public GeneralException(string message, Exception inner) : base(message, inner) { }
        
    }
}
