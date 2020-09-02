using FIL.Logging.Enums;
using System;
using System.Collections.Generic;

namespace FIL.Logging.Models
{
    public interface ILoggable
    {
        LogCategory LogCategory { get; set; }
        string Message { get; set; }
        Exception Exception { get; set; }
        Guid LogGuid { get; set; }
        int? SessionId { get; set; }
        int? UserId { get; set; }
        IDictionary<string, object> CustomVariables { get; set; }
    }

    public class Loggable : ILoggable
    {
        public LogCategory LogCategory { get; set; }
        public string Message { get; set; }
        public Exception Exception { get; set; }
        public Guid LogGuid { get; set; }
        public int? SessionId { get; set; }
        public int? UserId { get; set; }
        public IDictionary<string, object> CustomVariables { get; set; }
    }
}