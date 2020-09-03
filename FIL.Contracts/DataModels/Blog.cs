using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class Blog : IId<long>, IAuditable, IAuditableDates
    {
        public long Id { get; set; }
        public int BlogId { get; set; }
        public string ImageUrl { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class BlogValidator : AbstractValidator<Blog>, IFILValidator
    {
        public BlogValidator()
        {
            RuleFor(s => s.BlogId).NotEmpty().WithMessage("Blog Id is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}