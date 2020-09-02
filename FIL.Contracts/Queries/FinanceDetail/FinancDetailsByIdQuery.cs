using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResult;
using System;

namespace FIL.Contracts.Queries.FinanceDetail
{
    public class FinancDetailsByIdQuery : IQuery<FinancDetailsByIdQueryResults>
    {
        public Guid EventId { get; set; }
    }
}