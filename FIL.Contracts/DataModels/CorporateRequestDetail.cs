using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class CorporateRequestDetail : IId<long>, IAuditable, IAuditableDates
    {
        public long Id { get; set; }
        public long CorporateRequestId { get; set; }
        public long EventTicketAttributeId { get; set; }
        public int TicketTypeId { get; set; }
        public int? RequestedTickets { get; set; }
        public int? ApprovedTickets { get; set; }
        public decimal Price { get; set; }
        public bool ApprovedStatus { get; set; }
        public Guid? ApprovedBy { get; set; }
        public DateTime? ApprovedUtc { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class CorporateRequestDetailValidator : AbstractValidator<CorporateRequestDetail>, IFILValidator
    {
        public CorporateRequestDetailValidator()
        {
            RuleFor(s => s.CorporateRequestId).NotEmpty().WithMessage("CorporateRequestId is required");
            RuleFor(s => s.EventTicketAttributeId).NotEmpty().WithMessage("EventTicketDetailId is required");
            RuleFor(s => s.RequestedTickets).NotEmpty().WithMessage("RequestedTickets is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}