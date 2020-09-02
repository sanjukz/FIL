using System;

namespace FIL.Contracts.Exceptions
{
    public class UnhandledException : Exception
    {
        public Guid LogGuid { get; set; }

        public UnhandledException(string message) : base(message)
        {
        }

        public UnhandledException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}