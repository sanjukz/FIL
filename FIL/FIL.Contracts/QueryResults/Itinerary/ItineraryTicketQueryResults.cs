using FIL.Contracts.Models;
using System;
using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.ItineraryTicket
{
    public class ItineraryTicketQueryResult
    {
        public List<ItineraryTicketDetails> itineraryTicketDetails = new List<ItineraryTicketDetails>();
    }

    public class ItineraryTicketDetails
    {
        public Int64 eventId { get; set; }
        public List<EventDetails> eventDetails { get; set; }
        public List<EventTicketDetail> eventTicketDetails { get; set; }
        public List<TicketCategory> ticketCategories { get; set; }
        public List<EventTicketAttribute> eventTicketAttributes { get; set; }
    }
}