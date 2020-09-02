namespace FIL.Contracts.Models
{
    public class SubCategory
    {
        public SectionDetail SectionDetails { get; set; }
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public string Slug { get; set; }
        public string Url { get; set; }
        public string Query { get; set; }
        public bool IsMainCategory { get; set; }
        public int Order { get; set; }
        public FIL.Contracts.Enums.MasterEventType MasterEventTypeId { get; set; }
    }
}