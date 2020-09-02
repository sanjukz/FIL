using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class PlaceTicketRedemptionDetail : IId<long>, IAuditable, IAuditableDates
    {
        public long Id { get; set; }
        public long EventDetailId { get; set; }
        public string RedemptionsInstructions { get; set; }
        public string RedemptionsAddress { get; set; }
        public string RedemptionsCity { get; set; }
        public string RedemptionsState { get; set; }
        public string RedemptionsCountry { get; set; }
        public string RedemptionsZipcode { get; set; }
        public DateTime RedemptionsDateTime { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }
}