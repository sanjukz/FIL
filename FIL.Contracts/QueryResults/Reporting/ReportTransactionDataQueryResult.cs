using FIL.Contracts.Models;
using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.Reporting
{
    public class ReportTransactionDataQueryResult
    {
        public IEnumerable<FIL.Contracts.Models.Transaction> Transaction { get; set; }
        public IEnumerable<TransactionDetail> TransactionDetail { get; set; }
        public IEnumerable<TransactionDeliveryDetail> TransactionDeliveryDetail { get; set; }
        public IEnumerable<TransactionPaymentDetail> TransactionPaymentDetail { get; set; }
        public IEnumerable<CurrencyType> CurrencyType { get; set; }
        public IEnumerable<FIL.Contracts.Models.User> User { get; set; }
        public IEnumerable<FIL.Contracts.Models.UserCardDetail> UserCardDetail { get; set; }
        public IEnumerable<FIL.Contracts.Models.IPDetail> IPDetail { get; set; }
        public FIL.Contracts.Models.Pagination Pagination { get; set; }
    }
}