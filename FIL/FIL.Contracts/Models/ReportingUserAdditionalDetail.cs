using System;

namespace FIL.Contracts.Models
{
    public class ReportingUserAdditionalDetail
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public bool IsCredentialsMailed { get; set; }
        public string ProfilePic { get; set; }
        public string ClientLogo { get; set; }
        public bool IsEnabled { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedUtc { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedUtc { get; set; }
    }
}