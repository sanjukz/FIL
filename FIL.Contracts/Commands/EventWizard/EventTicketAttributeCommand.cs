using FIL.Contracts.Enums;
using System;

namespace FIL.Contracts.Commands.EventWizard
{
    public class EventTicketAttributeCommand : BaseCommand
    {
        public DateTime SalesStartDateTime { get; set; }
        public DateTime SalesEndDatetime { get; set; }
        public TicketType TicketTypeId { get; set; }
        public Channels ChannelId { get; set; }
        public int CurrencyId { get; set; }
        public short? SharedInventoryGroupId { get; set; }
        public short AvailableTicketForSale { get; set; }
        public short RemainingTicketForSale { get; set; }
        public string TicketCategoryDescription { get; set; }
        public string ViewFromStand { get; set; }
        public bool? IsSeatSelection { get; set; }
        public decimal Price { get; set; }
        public bool? IsInternationalCardAllowed { get; set; }
        public bool? IsEMIApplicable { get; set; }
    }
}