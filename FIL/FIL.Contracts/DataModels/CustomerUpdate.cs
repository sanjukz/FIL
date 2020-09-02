using FluentValidation;
using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class CustomerUpdate : IId<int>, IAuditable, IAuditableDates
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public short SortOrder { get; set; }
        public Site SiteId { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class CustomerUpdateValidator : AbstractValidator<CustomerUpdate>, IKzValidator
    {
        public CustomerUpdateValidator()
        {
            RuleFor(s => s.SortOrder).NotEmpty().WithMessage("SortOrder is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}