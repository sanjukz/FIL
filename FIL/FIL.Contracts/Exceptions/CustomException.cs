using System;
using System.Collections.Generic;

namespace FIL.Contracts.Exceptions
{
    public class CustomException : Exception
    {
        public Guid LogGuid { get; set; }

        public string CustomStackTrace { get; set; }

        public Dictionary<string, object> Variables { get; set; }

        public CustomException()
        {
        }

        public CustomException(string message)
            : base(message)
        {
        }

        public CustomException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public CustomException(string message, Dictionary<string, object> variables)
            : this(message)
        {
            Variables = variables;
        }

        public override string StackTrace => CustomStackTrace ?? base.StackTrace;
    }
}