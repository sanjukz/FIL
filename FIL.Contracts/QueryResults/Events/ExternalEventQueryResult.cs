using FIL.Contracts.Models;

namespace FIL.Contracts.QueryResults.Events
{
    public class ExternalEventQueryResult
    {
        public EventContainer EventContainer { get; set; }
        public bool IsValid { get; set; }
        public string ErrorMessage { get; set; }
    }
}