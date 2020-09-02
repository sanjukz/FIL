using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.BoxOffice;
using System;

namespace FIL.Contracts.Queries.BoxOffice
{
    public class ReprintRequestDataQuery : IQuery<ReprintRequestDataQueryResult>
    {
        public Guid AltId { get; set; }
    }
}