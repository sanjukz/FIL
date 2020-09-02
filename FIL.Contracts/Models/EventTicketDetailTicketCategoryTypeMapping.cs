namespace FIL.Contracts.Models
{
    public class EventTicketDetailTicketCategoryTypeMapping
    {
        public long Id { get; set; }
        public long EventTicketDetailId { get; set; }
        public long TicketCategorySubTypeId { get; set; }
        public long TicketCategoryTypeId { get; set; }
    }
}