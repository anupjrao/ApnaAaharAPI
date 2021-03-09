using System;
using System.Collections.Generic;
using System.Text;

namespace ApnaAahar.Exceptions
{
    public class DataNotSavedException : Exception
    {
            public DataNotSavedException() { }
            public DataNotSavedException(string message) : base(message) { }
            public DataNotSavedException(string message, Exception inner) : base(message, inner) { }
           
    }
}
