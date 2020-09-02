using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.Reporting.Graph;
using System;
using System.Collections.Generic;

namespace FIL.Contracts.Queries.Reporting.Graph
{
    public class GetMultiSubeventQuery : IQuery<GetMultiSubeventQueryResult>
    {
        public List<Guid> EventAltIds { get; set; }
        public List<Guid> VenueAltIds { get; set; }
    }
}