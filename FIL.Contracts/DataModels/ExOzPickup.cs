using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class ExOzPickup : IId<long>, IAuditable
    {
        public long Id { get; set; }
        public long PickupId { get; set; }
        public string Location { get; set; }
        public string Type { get; set; }
        public long PickupKey { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class ExOzPickupDetailValidator : AbstractValidator<ExOzPickup>, IKzValidator
    {
        public ExOzPickupDetailValidator()
        {
            RuleFor(s => s.PickupId).NotEmpty().WithMessage("PickupId is required");
            RuleFor(s => s.Location).NotEmpty().WithMessage("Location is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}