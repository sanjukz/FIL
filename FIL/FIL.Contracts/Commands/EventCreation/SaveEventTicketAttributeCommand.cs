using FIL.Contracts.Enums;
using System;

namespace FIL.Contracts.Commands.EventCreation
{
    public class SaveEventTicketAttributeCommand : Contracts.Interfaces.Commands.ICommandWithResult<SaveEventTicketAttributeResult>
    {
        public long Id { get; set; }
        public Guid AltId { get; set; }
        public long EventTicketDetailId { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime SalesStartDateTime { get; set; }
        public DateTime SalesEndDatetime { get; set; }
        public TicketType TicketTypeId { get; set; }
        public Channels ChannelId { get; set; }
        public int CurrencyId { get; set; }
        public short AvailableTicketForSale { get; set; }
        public short RemainingTicketForSale { get; set; }
        public string TicketCategoryDescription { get; set; }
        public string ViewFromStand { get; set; }
        public bool IsSeatSelection { get; set; }
        public decimal price { get; set; }
        public decimal? LocalPrice { get; set; }
        public bool? SeasonPackage { get; set; }
        public decimal? SeasonPackagePrice { get; set; }
        public int? LocalCurrencyId { get; set; }
        public bool? IsInternationalCardAllowed { get; set; }
        public bool? IsEMIApplicable { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class SaveEventTicketAttributeResult : Contracts.Interfaces.Commands.ICommandResult
    {
        public long Id { get; set; }
        public Guid ModifiedBy { get; set; }
    }
}