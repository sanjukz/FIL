namespace FIL.Contracts.Models
{
    public class EventCategoryMapping
    {
        public int Id { get; set; }
        public int EventCategoryId { get; set; }
        public long EventId { get; set; }
        public bool IsEnabled { get; set; }
    }
}