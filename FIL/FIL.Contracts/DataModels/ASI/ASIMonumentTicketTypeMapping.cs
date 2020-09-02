using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels.ASI
{
    public class ASIMonumentTicketTypeMapping : IId<long>, IAuditable, IAuditableDates
    {
        public long Id { get; set; }
        public long ASIMonumentDetailId { get; set; }
        public int ASITicketTypeId { get; set; }
        public decimal Total { get; set; }
        public decimal ASI { get; set; }
        public decimal LDA { get; set; }
        public decimal Others { get; set; }
        public decimal MSM { get; set; }
        public decimal AC { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }
}