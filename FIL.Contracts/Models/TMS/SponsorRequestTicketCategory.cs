namespace FIL.Contracts.Models.TMS
{
    public class SponsorRequestTicketCategory
    {
        public string TicketCategoryName { get; set; }
        public decimal? LocalPrice { get; set; }
        public string CurrencyName { get; set; }
        public int? RemainingTicketForSale { get; set; }
        public long EventTicketAttributeId { get; set; }
        public int RequestedTickets { get; set; }
    }
}