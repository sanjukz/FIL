using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.EventCreation;
using System;

namespace FIL.Contracts.Queries.EventCreation
{
    public class EventDataQuery : IQuery<EventDataQueryResult>
    {
        public Guid EventAltId { get; set; }
    }
}