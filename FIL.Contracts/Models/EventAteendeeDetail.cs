using System;

namespace FIL.Contracts.Models
{
    public class EventAteendeeDetail
    {
        public long TransactionId { get; set; }
        public long? TransactionDetailId { get; set; }
        public long EventFormFieldId { get; set; }
        public string Value { get; set; }
        public short AttendeeNumber { get; set; }
        public DateTime CreatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
    }
}