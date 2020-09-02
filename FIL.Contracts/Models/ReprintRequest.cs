using FIL.Contracts.Enums;
using System;

namespace FIL.Contracts.Models
{
    public class ReprintRequest
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long TransactionId { get; set; }
        public DateTime RequestDateTime { get; set; }
        public string Remarks { get; set; }
        public bool IsApproved { get; set; }
        public long? ApprovedBy { get; set; }
        public DateTime? ApprovedDateTime { get; set; }
        public Modules ModuleId { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        public Guid? AltId { get; set; }
    }
}