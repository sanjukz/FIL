using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class ZsuiteUserFeeDetail : IId<long>, IAuditable, IAuditableDates
    {
        public long Id { get; set; }
        public long BoxOfficeUserAdditionalDetailId { get; set; }
        public int ZsuitePaymentOptionId { get; set; }
        public string Fee { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class ZsuiteUserFeeDetailValidator : AbstractValidator<ZsuiteUserFeeDetail>, IFILValidator
    {
        public ZsuiteUserFeeDetailValidator()
        {
            RuleFor(s => s.Fee).NotEmpty().WithMessage("Fee is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}