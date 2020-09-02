using System;

namespace FIL.Contracts.Models.CitySightSeeing
{
    public class CitySightSeeingTicketDetail
    {
        public int Id { get; set; }
        public Guid AltId { get; set; }
        public string TicketId { get; set; }
        public string Title { get; set; }
        public string Duration { get; set; }
        public string BookSizeMin { get; set; }
        public string BookSizeMax { get; set; }
        public string SupplierUrl { get; set; }
        public int TicketClass { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string BookingStartDate { get; set; }
        public string Currency { get; set; }
        public string PickupPoints { get; set; }
        public string CombiTicket { get; set; }
    }
}