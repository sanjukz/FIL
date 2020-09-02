using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.TransactionStripeConnectTransfers;
using System;

namespace FIL.Contracts.Queries.TransactionStripeConnectTransfers
{
    public class TransactionStripeConnectTransfersQuery : IQuery<TransactionStripeConnectTransfersQueryResult>
    {
        public DateTime TransferDate { get; set; }
    }
}