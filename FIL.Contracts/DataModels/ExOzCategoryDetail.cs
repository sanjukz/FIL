using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class ExOzCategoryDetail : IId<int>, IAuditable
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int RegionId { get; set; }
        public string Name { get; set; }
        public string UrlSegment { get; set; }
        public int? OperatorQty { get; set; }
        public int? Offset { get; set; }
        public string TimeStamp { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class ExOzCategoryDetailValidator : AbstractValidator<ExOzCategoryDetail>, IFILValidator
    {
        public ExOzCategoryDetailValidator()
        {
            RuleFor(s => s.CategoryId).NotEmpty().WithMessage("CategoryId is required");
            RuleFor(s => s.RegionId).NotEmpty().WithMessage("RegionId is required");
            RuleFor(s => s.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(s => s.UrlSegment).NotEmpty().WithMessage("UrlSegment is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}