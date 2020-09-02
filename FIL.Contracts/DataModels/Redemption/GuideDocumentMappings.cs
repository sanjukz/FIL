using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels.Redemption
{
    public class Redemption_GuideDocumentMappings : IId<long>, IAuditable, IAuditableDates
    {
        public long Id { get; set; }
        public long GuideId { get; set; }
        public long DocumentID { get; set; }
        public string DocumentSouceURL { get; set; }
        public bool IsEnabled { get; set; }

        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }
}