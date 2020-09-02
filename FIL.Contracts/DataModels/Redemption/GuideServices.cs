using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels.Redemption
{
    public class Redemption_GuideServices : IId<long>, IAuditable, IAuditableDates
    {
        public long Id { get; set; }
        public long GuideId { get; set; }
        public int ServiceId { get; set; }
        public string Notes { get; set; }
        public bool IsEnabled { get; set; }

        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }
}