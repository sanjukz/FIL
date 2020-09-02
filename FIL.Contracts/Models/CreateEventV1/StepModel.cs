namespace FIL.Contracts.Models.CreateEventV1
{
    public class StepModel
    {
        public int StepId { get; set; }
        public string Name { get; set; }
        public int SortOrder { get; set; }
        public string Icon { get; set; }
        public string Description { get; set; }
        public string Slug { get; set; }
        public bool IsEnabled { get; set; }
    }
}