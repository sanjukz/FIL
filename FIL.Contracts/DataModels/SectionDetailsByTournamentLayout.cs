namespace FIL.Contracts.DataModels
{
    public class SectionDetailsByTournamentLayout
    {
        public int SectionId { get; set; }
        public string SectionName { get; set; }
        public int MasterVenueLayoutSectionId { get; set; }
        public int LayoutId { get; set; }
        public int TournamentLayoutSectionId { get; set; }
        public int Capacity { get; set; }
        public string GateName { get; set; }
        public int SeatStatus { get; set; }
        public int ChildCount { get; set; }
        public int IsTournamentExists { get; set; }
        public short VenueLayoutAreaId { get; set; }
        public int TicketCategoryId { get; set; }
        public decimal Price { get; set; }
        public int AvailableTicketForSale { get; set; }
        public bool IsSeatSelection { get; set; }
        public int CurrencyId { get; set; }
        public int LocalCurrencyId { get; set; }
        public decimal LocalPrice { get; set; }
        public int EntryGateId { get; set; }
    }
}