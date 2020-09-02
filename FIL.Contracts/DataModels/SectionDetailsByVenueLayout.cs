namespace FIL.Contracts.DataModels
{
    public class SectionDetailsByVenueLayout
    {
        public int SectionId { get; set; }
        public string SectionName { get; set; }
        public int MasterVenueLayoutSectionId { get; set; }
        public int LayoutId { get; set; }
        public int Capacity { get; set; }
        public string GateName { get; set; }
        public int SeatStatus { get; set; }
        public int ChildCount { get; set; }
    }
}