namespace FIL.Contracts.Models
{
    public class ReprintRequestContainer
    {
        public MatchSeatTicketDetail GetMatchSeatTicketDetail { get; set; }
        public EventDetail EventDetail { get; set; }
        public TicketCategory TicketCategory { get; set; }
        public City City { get; set; }
    }
}