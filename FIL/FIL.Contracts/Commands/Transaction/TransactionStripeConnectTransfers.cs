using System;

namespace FIL.Contracts.Commands.TransactionStripeConnectTransfers
{
    public class TransactionStripeConnectTransfersCommand : BaseCommand
    {
        public long Id { get; set; }
        public string TransferApiResponse { get; set; }
        public DateTime TransferDateActual { get; set; }
        public Guid ModifiedBy { get; set; }
    }
}