using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.Events;
using System;

namespace FIL.Contracts.Queries.Events
{
    public class EventDetailQuery : IQuery<EventDetailQueryResult>
    {
        public Guid EventAltId { get; set; }
        public Guid VenueAltId { get; set; }
    }
}