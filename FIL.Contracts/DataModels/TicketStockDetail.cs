using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class TicketStockDetail : IId<long>, IAuditable, IAuditableDates
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string TicketStockStartSrNo { get; set; }
        public string TicketStockEndSrNo { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class TicketStockDetailValidator : AbstractValidator<TicketStockDetail>, IKzValidator
    {
        public TicketStockDetailValidator()
        {
            RuleFor(s => s.UserId).NotEmpty().WithMessage("UserId is required");
            RuleFor(s => s.TicketStockStartSrNo).NotEmpty().WithMessage("TicketStockStartSrNo is required");
            RuleFor(s => s.TicketStockEndSrNo).NotEmpty().WithMessage("TicketStockEndSrNo is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}