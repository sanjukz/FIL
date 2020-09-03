using FluentValidation;
using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class EventFeeTypeMapping : IId<long>, IAuditable, IAuditableDates
    {
        public long Id { get; set; }
        public long EventId { get; set; }
        public Channels ChannelId { get; set; }
        public FeeType FeeId { get; set; }
        public FIL.Contracts.Enums.ValueTypes ValueTypeId { get; set; }
        public decimal Value { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class EventFeeTypeMappingsValidator : AbstractValidator<EventFeeTypeMapping>, IFILValidator
    {
        public EventFeeTypeMappingsValidator()
        {
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}