using System.Collections.Generic;

namespace FIL.Contracts.Models.TMS
{
    public class CategorySponsorDataModel
    {
        public List<SponsorModel> SponsorModels { get; set; }
        public List<SponsorEventDetailModel> SponsorEventDetailModels { get; set; }
        public CategorySponsorDetailModel CategorySponsorDetailModel { get; set; }
    }

    public class SponsorModel
    {
        public long EventTicketAttributeId { get; set; }
        public long CorporateTicketAllocationDetailId { get; set; }
        public long SponsorId { get; set; }
        public string SponsorName { get; set; }
        public int AllocatedTickets { get; set; }
        public int RemainingTickets { get; set; }
        public int ConsumedPaidTickets { get; set; }
        public int ConsumedComplimentaryTickets { get; set; }
        public int PendingPaidTickets { get; set; }
        public int PendingComplimentaryTickets { get; set; }
    }

    public class SponsorEventDetailModel
    {
        public long SponsorId { get; set; }
        public long EventDetailId { get; set; }
        public string EventDetailName { get; set; }
        public int RemainingTicketForPublicSale { get; set; }
        public int RemainingTickets { get; set; }
        public bool IsEnabled { get; set; }
    }

    public class CategorySponsorDetailModel
    {
        public long EventId { get; set; }
        public int VenueId { get; set; }
        public long EventTicketAttributeId { get; set; }
        public long TicketCategoryId { get; set; }
        public string TicketCategoryName { get; set; }
        public int AvailableTicketForSale { get; set; }
        public int RemainingTicketForSale { get; set; }
        public decimal Price { get; set; }
        public decimal LocalPrice { get; set; }
        public string CurrencyName { get; set; }
        public string LocalCurrencyName { get; set; }
    }
}