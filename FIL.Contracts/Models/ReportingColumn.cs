using System;

namespace FIL.Contracts.Models
{
    public class ReportingColumn
    {
        public long Id { get; set; }
        public string DBFieldName { get; set; }
        public string DisplayName { get; set; }
        public bool IsEnabled { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedUtc { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedUtc { get; set; }
    }
}