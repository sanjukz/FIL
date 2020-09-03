using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class Rating : IId<int>, IAuditable
    {
        public int Id { get; set; }
        public Guid AltId { get; set; }
        public long UserId { get; set; }
        public long EventId { get; set; }
        public short Points { get; set; }
        public string Comment { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class RatingValidator : AbstractValidator<Rating>, IFILValidator
    {
        public RatingValidator()
        {
            RuleFor(s => s.Points).NotEmpty().WithMessage("Points is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}