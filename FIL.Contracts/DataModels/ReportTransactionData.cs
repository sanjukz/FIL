using System.Collections.Generic;

namespace FIL.Contracts.DataModels
{
    public class ReportTransactionData
    {
        public IEnumerable<Transaction> Transaction { get; set; }
        public IEnumerable<TransactionDetail> TransactionDetail { get; set; }
        public IEnumerable<TransactionDeliveryDetail> TransactionDeliveryDetail { get; set; }
        public IEnumerable<TransactionPaymentDetail> TransactionPaymentDetail { get; set; }
        public IEnumerable<CurrencyType> CurrencyType { get; set; }
        public IEnumerable<User> User { get; set; }
        public IEnumerable<UserCardDetail> UserCardDetail { get; set; }
        public IEnumerable<IPDetail> IPDetail { get; set; }
        public IEnumerable<FIL.Contracts.Models.Pagination> Pagination { get; set; }
    }
}