using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class EventVenueMappingTime : IId<int>, IAuditable
    {
        public int Id { get; set; }
        public int EventVenueMappingId { get; set; }
        public string PickupTime { get; set; }
        public string PickupLocation { get; set; }
        public string ReturnTime { get; set; }
        public string ReturnLocation { get; set; }
        public int? JourneyType { get; set; }
        public string WaitingTime { get; set; }
        public bool IsEnabled { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedUtc { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedUtc { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }
}