namespace FIL.Contracts.Models.BoxOffice
{
    public class TicketCategoryContainer
    {
        public TicketCategory TicketCategory { get; set; }
        public EventTicketDetail EventTicketDetail { get; set; }
        public EventDetail EventDetail { get; set; }
        public EventTicketAttribute EventTicketAttribute { get; set; }
        public CurrencyType CurrencyType { get; set; }
    }
}