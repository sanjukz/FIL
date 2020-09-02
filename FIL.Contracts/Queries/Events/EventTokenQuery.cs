using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.Events;
using System;

namespace FIL.Contracts.Queries.Events
{
    public class EventTokenQuery : IQuery<EventTokenQueryResult>
    {
        public Guid AccessToken { get; set; }
    }
}