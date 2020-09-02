using System;

namespace FIL.Contracts.Models
{
    public class FloatDetail
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public decimal CashInHand { get; set; }
        public decimal CashInHandLocal { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
    }
}