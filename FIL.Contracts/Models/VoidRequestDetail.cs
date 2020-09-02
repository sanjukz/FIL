using System;

namespace FIL.Contracts.Models
{
    public class VoidRequestDetail
    {
        public long Id { get; set; }
        public long TransactionId { get; set; }
        public DateTime RequestDateTimeUtc { get; set; }
        public string Reason { get; set; }
        public bool IsVoid { get; set; }
        public DateTime? VoidDateTime { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
    }
}