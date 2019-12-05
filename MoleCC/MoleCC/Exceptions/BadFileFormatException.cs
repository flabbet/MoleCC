using System;
using System.Collections.Generic;
using System.Text;

namespace MoleCC.Exceptions
{
    public class BadFileFormatException : Exception
    {
        public BadFileFormatException(string message) : base(message)
        {

        }

        public BadFileFormatException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
