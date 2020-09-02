using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults;
using System;

namespace FIL.Contracts.Queries.Creator
{
    public class FILTransactionLocatorQuery : IQuery<FILTransactionQueryResult>
    {
        public long TransactionId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailId { get; set; }
        public string UserMobileNo { get; set; }
    }
}