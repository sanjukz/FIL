using FluentValidation;
using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class TransactionDetail : IId<long>, IAuditable, IAuditableDates
    {
        public long Id { get; set; }
        public long TransactionId { get; set; }
        public Guid? AltId { get; set; }
        public long EventTicketAttributeId { get; set; }
        public DateTime? VisitDate { get; set; }
        public short TotalTickets { get; set; }
        public decimal PricePerTicket { get; set; }
        public decimal? DeliveryCharges { get; set; }
        public decimal? ConvenienceCharges { get; set; }
        public decimal? ServiceCharge { get; set; }
        public decimal? DiscountAmount { get; set; }
        public short? TicketTypeId { get; set; }
        public string MembershipId { get; set; }
        public bool? IsRedeemed { get; set; }
        public long? ReferralId { get; set; }
        public DateTime? VisitEndDate { get; set; }
        public TransactionType TransactionType { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class TransactionDetailValidator : AbstractValidator<TransactionDetail>, IFILValidator
    {
        public TransactionDetailValidator()
        {
            RuleFor(s => s.TotalTickets).NotEmpty().WithMessage("TotalTickets are required");
            RuleFor(s => s.PricePerTicket).NotEmpty().WithMessage("PricePerTicket is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}