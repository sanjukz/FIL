using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class DTCMTransactionMapping : IId<long>, IAuditable
    {
        public long Id { get; set; }
        public Guid AltId { get; set; }
        public long TransactionId { get; set; }
        public long EventTicketDetailId { get; set; }
        public string OrderId { get; set; }
        public string BasketId { get; set; }
        public string BasketAmount { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class DTCMTransactionMappingValidator : AbstractValidator<DTCMTransactionMapping>, IKzValidator
    {
        public DTCMTransactionMappingValidator()
        {
            RuleFor(s => s.OrderId).NotEmpty().WithMessage("OrderId is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}