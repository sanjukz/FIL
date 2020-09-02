using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.EventLearnPage;
using System;

namespace FIL.Contracts.Queries.EventLearnPage
{
    public class EventLearnPageQuery : IQuery<WebEventLearnPageQueryResults>
    {
        public Guid EventAltId { get; set; }
    }
}