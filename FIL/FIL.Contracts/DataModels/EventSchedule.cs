using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class EventSchedule : IId<long>, IAuditable, IAuditableDates
    {
        public long Id { get; set; }
        public long EventId { get; set; }
        public FIL.Contracts.Enums.EventFrequencyType EventFrequencyTypeId { get; set; }
        public FIL.Contracts.Enums.OccuranceType OccuranceTypeId { get; set; }
        public string Name { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string DayId { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }
}