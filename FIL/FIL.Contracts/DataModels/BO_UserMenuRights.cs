using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class BO_UserMenuRights : IId<long>, IAuditable, IAuditableDates
    {
        public long Id { get; set; }
        public long User_Id { get; set; }
        public long MenuId { get; set; }
        public bool IsEnabled { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }
}