using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class MenuUserMapping : IId<long>, IAuditable
    {
        public long Id { get; set; }
        public int MenuId { get; set; }
        public long UserId { get; set; }
        public int SortOrder { get; set; }
        public bool IsEnabled { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedUtc { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedUtc { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class MenuUserMappingValidator : AbstractValidator<MenuUserMapping>, IKzValidator
    {
        public MenuUserMappingValidator()
        {
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}