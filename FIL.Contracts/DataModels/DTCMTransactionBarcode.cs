using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class DTCMTransactionBarcode : IId<long>, IAuditable
    {
        public long Id { get; set; }
        public Guid AltId { get; set; }
        public long DTCMTransactionMapId { get; set; }
        public string BarCode { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class DTCMTransactionBarcodeValidator : AbstractValidator<DTCMTransactionBarcode>, IFILValidator
    {
        public DTCMTransactionBarcodeValidator()
        {
            RuleFor(s => s.BarCode).NotEmpty().WithMessage("BarCode is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}