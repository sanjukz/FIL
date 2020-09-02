namespace FIL.Contracts.Models
{
    public class EventCategory
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public string DisplayName { get; set; }
        public string Slug { get; set; }
        public bool IsHomePage { get; set; }
        public int EventCategoryId { get; set; }
        public FIL.Contracts.Enums.MasterEventType MasterEventTypeId { get; set; }
        public bool IsFeel { get; set; }
        public int Order { get; set; }
    }
}