namespace FIL.Contracts.Models.TMS
{
    public class TicketCategoryContainer
    {
        public TicketCategory TicketCategory { get; set; }
        public EventTicketDetail EventTicketDetail { get; set; }
        public EventTicketAttribute EventTicketAttribute { get; set; }
    }
}