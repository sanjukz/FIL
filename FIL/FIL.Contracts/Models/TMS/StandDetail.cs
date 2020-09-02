namespace FIL.Contracts.Models.TMS
{
    public class StandDetail
    {
        public long EventId { get; set; }
        public long EventTicketAttributeId { get; set; }
        public decimal Price { get; set; }
        public decimal LocalPrice { get; set; }
        public decimal SeasonPackagePrice { get; set; }
        public decimal SeasonPackageLocalPrice { get; set; }
        public bool SeasonPackage { get; set; }
        public string CurrencyName { get; set; }
        public string LocalCurrencyName { get; set; }
        public string TicketCategoryName { get; set; }
        public int AvailableTicketForSale { get; set; }
        public int RemainingTickets { get; set; }
        public int TotalMatch { get; set; }
        public int TotalTicketSold { get; set; }
        public int TotalTicketBlocked { get; set; }
        public int SponsoredTickets { get; set; }
        public int SeatKills { get; set; }
        public int TotalTicketIssued { get; set; }
        public int ComplimentaryTicketIssued { get; set; }
        public int BlockUsed { get; set; }
        public int PublicSales { get; set; }
        public bool IsEnabled { get; set; }
    }
}