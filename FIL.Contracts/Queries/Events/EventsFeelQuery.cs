using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.Events;
using System;

namespace FIL.Contracts.Queries.Events
{
    public class EventsFeelQuery : IQuery<EventsFeelQueryResult>
    {
        public bool IsFeel { get; set; }
        public Guid UserAltId { get; set; }
        public int RoleId { get; set; }
    }
}