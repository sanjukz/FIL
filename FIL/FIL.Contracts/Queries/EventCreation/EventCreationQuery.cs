using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.EventCreation;
using System;

namespace FIL.Contracts.Queries.EventCreation
{
    public class EventCreationQuery : IQuery<EventCreationQueryResult>
    {
        public Guid EventAltId { get; set; }
    }
}