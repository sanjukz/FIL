using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class CorporateTicketAllocationDetail : IId<long>, IAuditable, IAuditableDates
    {
        public long Id { get; set; }
        public Guid AltId { get; set; }
        public long EventTicketAttributeId { get; set; }
        public long SponsorId { get; set; }
        public int AllocatedTickets { get; set; }
        public int RemainingTickets { get; set; }
        public decimal Price { get; set; }
        public bool IsCorporateRequest { get; set; }
        public long? CorporateRequestId { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class CorporateTicketAllocationDetailValidator : AbstractValidator<CorporateTicketAllocationDetail>, IFILValidator
    {
        public CorporateTicketAllocationDetailValidator()
        {
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}