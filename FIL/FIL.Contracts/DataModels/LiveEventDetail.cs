using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class LiveEventDetail : IId<int>, IAuditable, IAuditableDates
    {
        public int Id { get; set; }
        public long EventId { get; set; }
        public FIL.Contracts.Enums.OnlineEventTypes OnlineEventTypeId { get; set; }
        public FIL.Contracts.Enums.PerformanceType PerformanceTypeId { get; set; }
        public string VideoId { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsVideoUploaded { get; set; }
        public DateTime? EventStartDateTime { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }
}