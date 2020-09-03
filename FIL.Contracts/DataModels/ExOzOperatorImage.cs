using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class ExOzOperatorImage : IId<int>, IAuditable
    {
        public int Id { get; set; }
        public long? OperatorId { get; set; }
        public string ImageURL { get; set; }
        public string ImageType { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class ExOzOperatorImageValidator : AbstractValidator<ExOzOperatorImage>, IFILValidator
    {
        public ExOzOperatorImageValidator()
        {
            RuleFor(s => s.OperatorId).NotEmpty().WithMessage("OperatorId is required");
            RuleFor(s => s.ImageURL).NotEmpty().WithMessage("ImageURL is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}