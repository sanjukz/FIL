using FIL.Contracts.Enums;
using System;

namespace FIL.Contracts.Models
{
    public class EventTicketAttribute
    {
        public long Id { get; set; }
        public long EventTicketDetailId { get; set; }
        public DateTime SalesStartDateTime { get; set; }
        public DateTime SalesEndDatetime { get; set; }
        public TicketType TicketTypeId { get; set; }
        public Channels ChannelId { get; set; }
        public int CurrencyId { get; set; }
        public int? SharedInventoryGroupId { get; set; }
        public int AvailableTicketForSale { get; set; }
        public int RemainingTicketForSale { get; set; }
        public string TicketCategoryDescription { get; set; }
        public string ViewFromStand { get; set; }
        public bool? IsSeatSelection { get; set; }
        public decimal Price { get; set; }
        public decimal LocalPrice { get; set; }
        public int LocalCurrencyId { get; set; }
        public bool SeasonPackage { get; set; }
        public decimal SeasonPackagePrice { get; set; }
        public decimal SeasonPackageLocalPrice { get; set; }
        public bool? IsInternationalCardAllowed { get; set; }
        public string TicketValidity { get; set; }
        public string TicketValidityType { get; set; }
        public string TicketCategoryNotes { get; set; }
        public bool? IsEMIApplicable { get; set; }
        public short ChildQTY { get; set; }
        public short SRCitizenQTY { get; set; }
        public string ChildDiscount { get; set; }
        public string SrCitizenDiscount { get; set; }
        public string AdditionalInfo { get; set; }
        public decimal Specialprice { get; set; }
        public decimal SpecialSeasonPrice { get; set; }
    }
}