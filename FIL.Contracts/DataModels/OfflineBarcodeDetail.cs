using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class OfflineBarcodeDetail : IId<long>, IAuditable, IAuditableDates
    {
        public long Id { get; set; }
        public Guid AltId { get; set; }
        public long EventTicketDetailId { get; set; }
        public decimal Price { get; set; }
        public int CurrencyId { get; set; }
        public string BarcodeNumber { get; set; }
        public string AdditionalInfo { get; set; }
        public int? EntryCount { get; set; }
        public bool? EntryStatus { get; set; }
        public DateTime? EntryDateTime { get; set; }
        public DateTime? CheckedDateTime { get; set; }
        public int? EntryCountAllowed { get; set; }
        public bool IsConsumed { get; set; }
        public DateTime? ConsumedDateTime { get; set; }
        public bool IsEnabled { get; set; }
        public string EntryGateName { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class OfflineBarcodeDetailValidator : AbstractValidator<OfflineBarcodeDetail>, IFILValidator
    {
        public OfflineBarcodeDetailValidator()
        {
            RuleFor(s => s.BarcodeNumber).NotEmpty().WithMessage("BarcodeNumber is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}