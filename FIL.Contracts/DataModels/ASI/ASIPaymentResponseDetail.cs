using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels.ASI
{
    public class ASIPaymentResponseDetail : IId<long>, IAuditable, IAuditableDates
    {
        public long Id { get; set; }
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public string Error { get; set; }
        public string TransactionId { get; set; }
        public string PaymentId { get; set; }
        public string PaymentProvider { get; set; }
        public decimal PaymentAmount { get; set; }
        public string PaymentTransactionId { get; set; }
        public string PaymentGateway { get; set; }
        public DateTime PaymentTimeStamp { get; set; }
        public string PaymentStatus { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }
}