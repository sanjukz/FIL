namespace FIL.Contracts.Models
{
    public class GetTicketDataContainer
    {
        public Transaction Transaction { get; set; }
        public TransactionDetail TransactionDetail { get; set; }
        public MatchSeatTicketDetail MatchSeatTicketDetail { get; set; }
        public EventDetail EventDetail { get; set; }
        public TicketCategory TicketCategory { get; set; }
    }
}