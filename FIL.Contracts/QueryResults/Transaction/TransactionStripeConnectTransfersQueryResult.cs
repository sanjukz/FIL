using System;
using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.TransactionStripeConnectTransfers
{
    public class TransactionStripeConnectTransfersQueryResult
    {
        public List<TransactionStripeConnectTransfer> transactionStripeConnectTransfers { get; set; }
    }

    public class TransactionStripeConnectTransfer
    {
        public long Id { get; set; }
        public long TransactionId { get; set; }
        public int CurrencyId { get; set; }
        public long Amount { get; set; }
        public string StripeConnectedAccount { get; set; }
        public string SourceTransactionChargeId { get; set; }
        public DateTime TransferDateProposed { get; set; }
        public DateTime? TransferDateActual { get; set; }
        public string TransferApiResponse { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
    }
}