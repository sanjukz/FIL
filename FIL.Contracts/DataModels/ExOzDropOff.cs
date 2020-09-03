using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class ExOzDropOff : IId<long>, IAuditable
    {
        public long Id { get; set; }
        public long DropOffId { get; set; }
        public string Location { get; set; }
        public string Type { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class ExOzDropOffDetailValidator : AbstractValidator<ExOzDropOff>, IFILValidator
    {
        public ExOzDropOffDetailValidator()
        {
            RuleFor(s => s.DropOffId).NotEmpty().WithMessage("DropOffId is required");
            RuleFor(s => s.Location).NotEmpty().WithMessage("Location is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}