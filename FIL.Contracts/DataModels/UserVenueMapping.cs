using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class UserVenueMapping
    {
        public int Id { get; set; }
        public long UserId { get; set; }
        public int VenueId { get; set; }
        public bool IsEnabled { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedUtc { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedUtc { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class UserVenueMappingValidator : AbstractValidator<UserVenueMapping>, IFILValidator
    {
        public UserVenueMappingValidator()
        {
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}