using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class EventAttribute : IId<long>, IAuditable, IAuditableDates
    {
        public long Id { get; set; }
        public long EventDetailId { get; set; }
        public short? MatchNo { get; set; }
        public short? MatchDay { get; set; }
        public string GateOpenTime { get; set; }
        public string TimeZone { get; set; }
        public string TimeZoneAbbreviation { get; set; }
        public string TicketHtml { get; set; }
        public bool IsEnabled { get; set; }
        public string MatchAdditionalInfo { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class EventAttributeValidator : AbstractValidator<EventAttribute>, IFILValidator
    {
        public EventAttributeValidator()
        {
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}