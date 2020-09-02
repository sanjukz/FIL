using FluentValidation;
using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class MatchSeatTicketDetail : IId<long>, IAuditable, IAuditableDates
    {
        public Guid AltId { get; set; }
        public long Id { get; set; }
        public long? TransactionId { get; set; }
        public long? MatchLayoutSectionSeatId { get; set; }
        public long EventTicketDetailId { get; set; }
        public decimal Price { get; set; }
        public Channels? ChannelId { get; set; }
        public SeatStatus SeatStatusId { get; set; }
        public string BarcodeNumber { get; set; }
        public PrintStatus? PrintStatusId { get; set; }
        public int? PrintCount { get; set; }
        public string PrintedBy { get; set; }
        public DateTime? PrintDateTime { get; set; }
        public int? EntryCount { get; set; }
        public bool? EntryStatus { get; set; }
        public DateTime? EntryDateTime { get; set; }
        public DateTime? CheckedDateTime { get; set; }
        public int? EntryCountAllowed { get; set; }
        public TicketType TicketTypeId { get; set; }
        public int? IPDetailId { get; set; }
        public long? SponsorId { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsConsumed { get; set; }
        public DateTime? ConsumedDateTime { get; set; }
        public string EntryGateName { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class MatchSeatTicketDetailValidator : AbstractValidator<MatchSeatTicketDetail>, IKzValidator
    {
        public MatchSeatTicketDetailValidator()
        {
            RuleFor(s => s.Price).NotEmpty().WithMessage("Price is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}