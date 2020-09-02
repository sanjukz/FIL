using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.BoxOffice;

namespace FIL.Contracts.Queries.BoxOffice
{
    public class TransactionLocatorQuery : IQuery<TransactionLocatorQueryResult>
    {
        public long TransactionId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailId { get; set; }
        public string UserMobileNo { get; set; }
        public string BarcodeNumber { get; set; }
        public bool IsFulfilment { get; set; }
    }
}