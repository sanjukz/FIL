namespace FIL.Contracts.Models
{
    public class EventCategoryData
    {
        public int Order { get; set; }
        public string DisplayName { get; set; }
        public string Slug { get; set; }
        public bool IsHomePage { get; set; }
        public int CategoryId { get; set; }
        public bool IsFeel { get; set; }
        public int Value { get; set; }
        public FIL.Contracts.Enums.MasterEventType MasterEventTypeId { get; set; }
    }
}