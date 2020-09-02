using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults;
using System;

namespace FIL.Contracts.Queries.Venue

{
    public class EventVenueQuery : IQuery<EventVenueQueryResult>
    {
        public Guid AltId { get; set; }
        public Guid EventAltId { get; set; }
    }
}