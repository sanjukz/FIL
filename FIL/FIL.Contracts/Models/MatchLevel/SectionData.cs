namespace FIL.Contracts.Models.MatchLevel
{
    public class SectionData
    {
        public int Capacity { get; set; }
        public int ChildDiscount { get; set; }
        public int? Currency { get; set; }
        public string GateName { get; set; }
        public bool? IsChecked { get; set; }
        public bool? IsSeatSelection { get; set; }
        public bool IsTournamentExists { get; set; }
        public int LayoutId { get; set; }
        public int? LocalCurrency { get; set; }
        public int? LocalPrice { get; set; }
        public int MasterVenueLayoutSectionId { get; set; }
        public int? Price { get; set; }
        public int? SaleBy { get; set; }
        public int? SeatStatus { get; set; }
        public int SectionId { get; set; }
        public string SectionName { get; set; }
        public int? TournamentLayoutSectionId { get; set; }
        public int VenueLayoutAreaId { get; set; }
        public int InventoryTypeId { get; set; }
    }
}