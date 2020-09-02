using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels.ASI
{
    public class ASIMonumentDetail : IId<long>, IAuditable, IAuditableDates
    {
        public long Id { get; set; }
        public long ASIMonumentId { get; set; }
        public string Name { get; set; }
        public bool IsOptional { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }
}