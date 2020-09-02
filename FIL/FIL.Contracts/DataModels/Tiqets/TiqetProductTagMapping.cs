using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels.Tiqets
{
    public class TiqetProductTagMapping : IId<long>, IAuditable, IAuditableDates
    {
        public long Id { get; set; }
        public string ProductId { get; set; }
        public string TagId { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsCategoryType { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class TiqetProductTagMappingValidator : AbstractValidator<TiqetProductTagMapping>, IKzValidator
    {
        public TiqetProductTagMappingValidator()
        {
            RuleFor(s => s.ProductId).NotEmpty().WithMessage("Product Id is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}