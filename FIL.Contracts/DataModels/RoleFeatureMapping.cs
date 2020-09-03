using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class RoleFeatureMapping : IId<int>, IAuditable, IAuditableDates
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int FeatureId { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class RoleFeatureMappingValidator : AbstractValidator<RoleFeatureMapping>, IFILValidator
    {
        public RoleFeatureMappingValidator()
        {
            RuleFor(s => s.RoleId).NotEmpty().WithMessage("Role is required");
            RuleFor(s => s.FeatureId).NotEmpty().WithMessage("Feature is required");
        }
    }
}