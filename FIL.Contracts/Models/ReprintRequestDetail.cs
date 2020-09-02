using System;

namespace FIL.Contracts.Models
{
    public class ReprintRequestDetail
    {
        public long Id { get; set; }
        public long RePrintRequestId { get; set; }
        public long MatchSeatTicketDetaildId { get; set; }
        public string BarcodeNumber { get; set; }
        public bool IsRePrinted { get; set; }
        public int RePrintCount { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        public bool? IsApproved { get; set; }
        public DateTime? ApprovedDateTime { get; set; }
    }
}