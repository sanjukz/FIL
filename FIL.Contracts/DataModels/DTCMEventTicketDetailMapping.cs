using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class DTCMEventTicketDetailMapping : IId<int>, IAuditable
    {
        public int Id { get; set; }
        public Guid AltId { get; set; }
        public long EventDetailId { get; set; }
        public short? PriceCategoryId { get; set; }
        public string PriceCategoryCode { get; set; }
        public string PriceCategoryName { get; set; }
        public short? PriceTypeId { get; set; }
        public string PriceTypeCode { get; set; }
        public string PriceTypeName { get; set; }
        public string PriceTypeArea { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class DTCMEventTicketDetailMappingValidator : AbstractValidator<DTCMEventTicketDetailMapping>, IKzValidator
    {
        public DTCMEventTicketDetailMappingValidator()
        {
            RuleFor(s => s.PriceCategoryId).NotEmpty().WithMessage("PriceCategoryId is required");
            RuleFor(s => s.PriceTypeId).NotEmpty().WithMessage("PriceTypeId is required");
            RuleFor(s => s.PriceTypeCode).NotEmpty().WithMessage("PriceTypeCode is required");
            RuleFor(s => s.PriceTypeArea).NotEmpty().WithMessage("PriceTypeArea is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}