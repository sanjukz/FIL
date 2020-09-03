using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class PartialVoidRequestDetail : IId<long>, IAuditable, IAuditableDates
    {
        public long Id { get; set; }
        public string BarcodeNumber { get; set; }
        public DateTime RequestDateTimeUtc { get; set; }
        public bool IsPartialVoid { get; set; }
        public DateTime? PartialVoidDateTime { get; set; }
        public bool IsEnabled { get; set; }
        public string Reason { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class PartialVoidRequestDetailsValidator : AbstractValidator<PartialVoidRequestDetail>, IFILValidator
    {
        public PartialVoidRequestDetailsValidator()
        {
            RuleFor(s => s.BarcodeNumber).NotEmpty().WithMessage("BarcodeNumber is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}