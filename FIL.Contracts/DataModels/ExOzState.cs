using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class ExOzState : IId<int>, IAuditable
    {
        public int Id { get; set; }
        public int StateId { get; set; }
        public string Name { get; set; }
        public string UrlSegment { get; set; }
        public int? CountryId { get; set; }
        public int? StateMapId { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class ExOzStateValidator : AbstractValidator<ExOzState>, IFILValidator
    {
        public ExOzStateValidator()
        {
            RuleFor(s => s.StateId).NotEmpty().WithMessage("StateId is required");
            RuleFor(s => s.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(s => s.UrlSegment).NotEmpty().WithMessage("UrlSegment is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}