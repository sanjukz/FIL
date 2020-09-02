using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.EventHomePage;
using System;

namespace FIL.Contracts.Queries.EventHomePage
{
    public class EventHomePageQuery : IQuery<EventHomePageQueryResult>
    {
        public Guid EventAltId { get; set; }
    }
}