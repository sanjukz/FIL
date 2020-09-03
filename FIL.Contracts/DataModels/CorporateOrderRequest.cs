using FluentValidation;
using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class CorporateOrderRequest : IId<long>, IAuditable, IAuditableDates
    {
        public long Id { get; set; }
        public long SponsorId { get; set; }
        public OrderType OrderTypeId { get; set; }
        public AccountType AccountTypeId { get; set; }
        public int EventTicketAttributeId { get; set; }
        public int? RequestedTickets { get; set; }
        public int? AllocatedTickets { get; set; }
        public OrderStatus OrderStatusId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneCode { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class CorporateOrderRequestValidator : AbstractValidator<CorporateOrderRequest>, IFILValidator
    {
        public CorporateOrderRequestValidator()
        {
            RuleFor(s => s.SponsorId).NotEmpty().WithMessage("SponsorId is required");
            RuleFor(s => s.EventTicketAttributeId).NotEmpty().WithMessage("EventTicketDetailId is required");
            RuleFor(s => s.FirstName).NotEmpty().WithMessage("FirstName is required");
            RuleFor(s => s.LastName).NotEmpty().WithMessage("LastName is required");
            RuleFor(s => s.Email).NotEmpty().WithMessage("Email is required");
            RuleFor(s => s.PhoneCode).NotEmpty().WithMessage("PhoneCode is required");
            RuleFor(s => s.PhoneNumber).NotEmpty().WithMessage("PhoneNumber is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}