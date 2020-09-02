using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class FeelBarcodeMapping : IId<int>, IAuditable, IAuditableDates
    {
        public int Id { get; set; }
        public long TransactionDetailId { get; set; }
        public string Barcode { get; set; }
        public bool IsEnabled { get; set; }
        public bool GroupCodeExist { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class FeelBarcodeMappingValidator : AbstractValidator<EventCategoryMapping>, IKzValidator
    {
        public FeelBarcodeMappingValidator()
        {
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}