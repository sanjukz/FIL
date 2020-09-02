using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.Events;
using System;

namespace FIL.Contracts.Queries.Events
{
    public class ExternalEventQuery : IQuery<ExternalEventQueryResult>
    {
        public Guid EventAltId { get; set; }
    }
}