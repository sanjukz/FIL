using System;
using System.Collections.Generic;

namespace FIL.Contracts.Commands.TicketAlert
{
    public class TicketAlertUserMappingCommand : BaseCommand
    {
        public List<string> CountriesAltId { get; set; }
        public List<int> TicketAlertEvents { get; set; }
        public Guid EventAltId { get; set; }
        public long EventDetailId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneCode { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int NumberOfTickets { get; set; }
        public string EventName { get; set; }
        public FIL.Contracts.Enums.TourTravelPackages TourTravelPackages { get; set; }
        public bool IsStreamingEvent { get; set; }
        public string Ip { get; set; }
    }
}