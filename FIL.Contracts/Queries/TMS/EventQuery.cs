using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResult.TMS;
using System;

namespace FIL.Contracts.Queries.TMS
{
    public class EventQuery : IQuery<EventQueryResult>
    {
        public Guid VenueAltId { get; set; }
        public AllocationType AllocationType { get; set; }
    }
}