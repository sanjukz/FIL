using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class ExOzProductSession : IId<long>, IAuditable
    {
        public long Id { get; set; }
        public long ProductSessionId { get; set; }
        public string SessionName { get; set; }
        public long ProductId { get; set; }
        public string HasPickups { get; set; }
        public decimal Levy { get; set; }
        public bool IsExtra { get; set; }
        public string TourHour { get; set; }
        public string TourMinute { get; set; }
        public string TourDuration { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class ExOzProductSessionValidator : AbstractValidator<ExOzProductSession>, IKzValidator
    {
        public ExOzProductSessionValidator()
        {
            RuleFor(s => s.ProductSessionId).NotEmpty().WithMessage("ProductSessionId is required");
            RuleFor(s => s.SessionName).NotEmpty().WithMessage("SessionName is required");
            RuleFor(s => s.ProductId).NotEmpty().WithMessage("ProductId is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}