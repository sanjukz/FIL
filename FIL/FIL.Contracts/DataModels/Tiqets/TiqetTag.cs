using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels.Tiqets
{
    public class TiqetTag : IId<long>, IAuditable, IAuditableDates
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string TagId { get; set; }
        public string TagTypeId { get; set; }
        public int EventCategoryId { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class TiqetTagValidator : AbstractValidator<TiqetTag>, IKzValidator
    {
        public TiqetTagValidator()
        {
            RuleFor(s => s.Name).NotEmpty().WithMessage("Name Id is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}