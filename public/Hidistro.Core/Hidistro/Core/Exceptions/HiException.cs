namespace Hidistro.Core.Exceptions
{
    using System;

    public class HiException : ApplicationException
    {
        public HiException()
        {
        }

        public HiException(string message) : base(message)
        {
        }

        public HiException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

