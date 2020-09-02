using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class TransactionStripeConnectTransfers : IId<long>, IAuditable, IAuditableDates
    {
        public long Id { get; set; }
        public long TransactionId { get; set; }
        public int CurrencyId { get; set; }
        public long Amount { get; set; }
        public string StripeConnectedAccount { get; set; }
        public string SourceTransactionChargeId { get; set; }
        public Channels ChannelId { get; set; }
        public DateTime TransferDateProposed { get; set; }
        public DateTime? TransferDateActual { get; set; }
        public string TransferApiResponse { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }
}