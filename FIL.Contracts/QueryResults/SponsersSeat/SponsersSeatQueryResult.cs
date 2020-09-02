using FIL.Contracts.Models;
using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.SponsersSeat
{
    public class SponsersSeatQueryResult
    {
        public bool IsauthenticatedSponser { get; set; }
        public Event Event { get; set; }
        public IEnumerable<EventDetail> EventDetail { get; set; }
        public IEnumerable<Venue> Venue { get; set; }
        public IEnumerable<City> City { get; set; }
        public IEnumerable<Team> Team { get; set; }
        public IEnumerable<MatchAttribute> MatchAttribute { get; set; }
        public IEnumerable<TicketCategory> TicketCategory { get; set; }
        public IEnumerable<TicketCategory> SponserAssociatedTicketCategory { get; set; }
        public IEnumerable<EventTicketDetail> SponserAssociatedEventTicketDetails { get; set; }
        public IEnumerable<MatchSeatTicketDetail> PartialViewSponsors { get; set; }
        public IEnumerable<MatchSeatTicketDetail> KillViewSponsors { get; set; }
        public IEnumerable<EventTicketAttribute> EventTicketAttribute { get; set; }
        public IEnumerable<TicketFeeDetail> TicketFeeDetail { get; set; }
        public IEnumerable<EventTicketDetail> EventTicketDetail { get; set; }
        public IEnumerable<RASVTicketTypeMapping> RASVTicketTypeMappings { get; set; }
        public IEnumerable<EventDeliveryTypeDetail> EventDeliveryTypeDetails { get; set; }
        public IEnumerable<MatchSeatTicketDetail> matchSeatTicketDetails { get; set; }
        public IEnumerable<MatchLayoutSectionSeat> matchLayoutSectionSeats { get; set; }
        public CurrencyType CurrencyType { get; set; }
        public string EventCategoryName { get; set; }
        public int EventCategory { get; set; }
    }
}