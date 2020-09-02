using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.TransactionReport;
using System;

namespace FIL.Contracts.Queries.TransactionReport
{
    public class FAPGetPlacesQuery : IQuery<FAPGetPlacesQueryResult>
    {
        public bool isFeel { get; set; }
        public Guid UserAltId { get; set; }
    }
}