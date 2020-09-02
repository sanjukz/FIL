using System;

namespace FIL.Contracts.Models
{
    public class PartialVoidRequestDetail
    {
        public long Id { get; set; }
        public string BarcodeNumber { get; set; }
        public DateTime RequestDateTimeUtc { get; set; }
        public bool IsPartialVoid { get; set; }
        public DateTime? PartialVoidDateTime { get; set; }
        public string Reason { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
    }
}