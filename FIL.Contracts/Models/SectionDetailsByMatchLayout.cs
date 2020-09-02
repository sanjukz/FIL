namespace FIL.Contracts.Models
{
    public class SectionDetailsByMatchLayout
    {
        public int SectionId { get; set; }
        public string SectionName { get; set; }
        public int TournamentLayoutSectionId { get; set; }
        public int MatchLayoutId { get; set; }
        public int MatchLayoutSectionId { get; set; }
        public int Capacity { get; set; }
        public string GateName { get; set; }
        public int SeatStatus { get; set; }
        public int ChildCount { get; set; }
        public int IsMatchExists { get; set; }
        public int VenueAreaLayoutId { get; set; }
        public decimal Price { get; set; }
        public int AvailableTicketForSale { get; set; }
        public bool IsSeatSelection { get; set; }
        public int CurrencyId { get; set; }
        public int LocalCurrencyId { get; set; }
        public decimal LocalPrice { get; set; }
        public int EntryGateId { get; set; }
    }
}