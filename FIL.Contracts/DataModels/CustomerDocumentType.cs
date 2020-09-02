using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class CustomerDocumentType : IId<long>, IAuditable, IAuditableDates
    {
        public long Id { get; set; }
        public string DocumentType { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsASI { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }
}