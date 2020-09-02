using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels.Tiqets
{
    public class TiqetEventTicketDetailMapping : IId<long>, IAuditable, IAuditableDates
    {
        public long Id { get; set; }
        public long TiqetVariantDetailId { get; set; }
        public long EventTicketDetailId { get; set; }
        public bool IsEnabled { get; set; }
        public string ProductId { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class TiqetEventTicketDetailMappingValidator : AbstractValidator<TiqetEventTicketDetailMapping>, IKzValidator
    {
        public TiqetEventTicketDetailMappingValidator()
        {
            RuleFor(s => s.TiqetVariantDetailId).NotEmpty().WithMessage("Product Id is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}