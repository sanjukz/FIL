using System;

namespace FIL.Contracts.QueryResults.Events
{
    public class EventTokenQueryResult
    {
        public bool IsValid { get; set; }
        public Guid EventAltId { get; set; }
    }
}