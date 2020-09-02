using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class EventStripeConnectMaster : IId<int>, IAuditable, IAuditableDates
    {
        public int Id { get; set; }
        public long EventId { get; set; }
        public string StripeConnectAccountID { get; set; }
        public Decimal ExtraCommisionFlat { get; set; }
        public Decimal ExtraCommisionPercentage { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }
}