using System.Collections.Generic;

namespace FIL.Contracts.Models.Integrations.HttpHelpers
{
    public interface IRequestCreateOptions<T>
    {
        Dictionary<string, string> Header { get; set; }
        T Content { get; set; }
        string ContentType { get; set; }
        string Accept { get; set; }
        string UserAgent { get; set; }
    }

    public class RequestCreateOptions<T> : IRequestCreateOptions<T>
    {
        public Dictionary<string, string> Header { get; set; }
        public T Content { get; set; }
        public string ContentType { get; set; } = "application/json";
        public string Accept { get; set; }
        public string UserAgent { get; set; }
    }
}