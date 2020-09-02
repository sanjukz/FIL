using System;

namespace FIL.Contracts.Models
{
    public class TransactionDeliveryDetail
    {
        public long Id { get; set; }
        public long TransactionDetailId { get; set; }
        public string DeliveryTypeId { get; set; }
        public short? PickupBy { get; set; }
        public string SecondaryName { get; set; }
        public string PickUpAddress { get; set; }
        public string SecondaryContact { get; set; }
        public string SecondaryEmail { get; set; }
        public string CourierAddress { get; set; }
        public int? CourierZipcode { get; set; }
        public bool? DeliveryStatus { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string DeliverdTo { get; set; }
        public string CourierServiceId { get; set; }
        public string TrackingId { get; set; }
    }
}