using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.Reporting.Graph;
using System.Collections.Generic;

namespace FIL.Contracts.Queries.Reporting.Graph
{
    public class GetMultiTicketCategoryQuery : IQuery<GetMultiTicketCategoryQueryResult>
    {
        public List<long> EventDetailIds { get; set; }
    }
}