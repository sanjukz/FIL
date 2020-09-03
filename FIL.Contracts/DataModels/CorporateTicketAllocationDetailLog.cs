using FluentValidation;
using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class CorporateTicketAllocationDetailLog : IId<long>, IAuditable, IAuditableDates
    {
        public long Id { get; set; }
        public long CorporateTicketAllocationDetailId { get; set; }
        public long? TransferToCorporateTicketAllocationDetailId { get; set; }
        public AllocationOption AllocationOptionId { get; set; }
        public short TotalTickets { get; set; }
        public decimal Price { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class CorporateTicketAllocationDetailLogValidator : AbstractValidator<CorporateTicketAllocationDetailLog>, IFILValidator
    {
        public CorporateTicketAllocationDetailLogValidator()
        {
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}