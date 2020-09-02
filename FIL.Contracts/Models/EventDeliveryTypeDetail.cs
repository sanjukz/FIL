using System;

namespace FIL.Contracts.Models
{
    public class EventDeliveryTypeDetail
    {
        public long Id { get; set; }
        public long EventDetailId { get; set; }
        public string DeliveryTypeId { get; set; }
        public string Notes { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsEnabled { get; set; }
    }
}