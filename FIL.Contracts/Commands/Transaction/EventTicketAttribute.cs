using FIL.Contracts.Enums;
using FIL.Contracts.Models.ASI;
using System;
using System.Collections.Generic;

namespace FIL.Contracts.Commands.Transaction
{
    public class EventTicketAttribute
    {
        public long Id { get; set; }
        public short TotalTickets { get; set; }
        public TicketType TicketType { get; set; }
        public TransactionType TransactionType { get; set; }
        public long EventDetailId { get; set; }
        public DateTime VisitDate { get; set; }
        public MoveAroundBookingAddress PurchaserAddress { get; set; }
        public List<GuestUserDetail> GuestDetails { get; set; }
        public int? VisitTimeId { get; set; }
        public int EventVenueMappingTimeId { get; set; }
        public bool IsAdult { get; set; }
        public String ASIUserSelectedCountry { get; set; }
        public string MembershipId { get; set; }
        public string VisitStartTime { get; set; }
        public string VisitEndTime { get; set; }
        public decimal Price { get; set; }
        public string TimeSlot { get; set; }
        public decimal DiscountedPrice { get; set; }
        public bool? ReserveHohoBook { get; set; }
        public decimal? DonationAmount { get; set; }
        public ASIAvailability ASIAvailability { get; set; }
        public string eventAltId { get; set; }
        public decimal? OverridedAmount { get; set; }
        public long? ScheduleDetailId { get; set; }
    }
}