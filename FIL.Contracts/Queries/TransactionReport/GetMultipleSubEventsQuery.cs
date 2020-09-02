using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.TransactionReport;
using System;
using System.Collections.Generic;

namespace FIL.Contracts.Queries.TransactionReport
{
    public class GetMultipleSubEventsQuery : IQuery<GetMultipleSubEventsQueryResult>
    {
        public List<Guid> EventAltIds { get; set; }
        public Guid UserAltId { get; set; }
    }
}