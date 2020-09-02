using FluentValidation;
using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class EventTicketAttribute : IId<long>, IAuditable, IAuditableDates
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
        public string TicketCategoryNotes { get; set; }
        public bool? IsSeatSelection { get; set; }
        public decimal Price { get; set; }
        public decimal LocalPrice { get; set; }
        public int LocalCurrencyId { get; set; }
        public bool SeasonPackage { get; set; }
        public decimal SeasonPackagePrice { get; set; }
        public decimal SeasonPackageLocalPrice { get; set; }
        public bool? IsInternationalCardAllowed { get; set; }
        public string AdditionalInfo { get; set; }
        public bool? IsEMIApplicable { get; set; }
        public bool IsEnabled { get; set; }
        public short ChildQTY { get; set; }
        public short SRCitizenQTY { get; set; }
        public string ChildDiscount { get; set; }
        public string SrCitizenDiscount { get; set; }
        public string TicketValidity { get; set; }
        public decimal Specialprice { get; set; }
        public decimal SpecialSeasonPrice { get; set; }
        public FIL.Contracts.Enums.TicketValidityTypes TicketValidityType { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class EventTicketAttributeValidator : AbstractValidator<EventTicketAttribute>, IKzValidator
    {
        public EventTicketAttributeValidator()
        {
            RuleFor(s => s.SalesStartDateTime).NotEmpty().WithMessage("SalesStartDateTime is required");
            RuleFor(s => s.SalesEndDatetime).NotEmpty().WithMessage("SalesEndDatetime is required");
            RuleFor(s => s.AvailableTicketForSale).NotEmpty().WithMessage("AvailableTicketForSale are required");
            RuleFor(s => s.RemainingTicketForSale).NotEmpty().WithMessage("RemainingTicketForSale are required");
            RuleFor(s => s.Price).NotEmpty().WithMessage("Price is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}