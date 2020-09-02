using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.Events;
using System;

namespace FIL.Contracts.Queries.Events
{
    public class EventQuery : IQuery<EventQueryResult>
    {
        public Guid AltId { get; set; }
    }
}