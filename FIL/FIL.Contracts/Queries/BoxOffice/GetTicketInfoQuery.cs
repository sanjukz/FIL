using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.BoxOffice;
using System;
using System.Collections.Generic;

namespace FIL.Contracts.Queries.BoxOffice
{
    public class GetTicketInfoQuery : IQuery<GetTicketInfoQueryResult>
    {
        public long TransactionId { get; set; }
        public Guid TransactionAltId { get; set; }
        public Guid ModifiedBy { get; set; }
        public bool IsRePrint { get; set; }
        public List<string> BarcodeNumbers { get; set; }
    }
}