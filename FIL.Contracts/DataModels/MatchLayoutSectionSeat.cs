using FluentValidation;
using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class MatchLayoutSectionSeat : IId<long>, IAuditable, IAuditableDates
    {
        public long Id { get; set; }
        public string SeatTag { get; set; }
        public int MatchLayoutSectionId { get; set; }
        public string RowNumber { get; set; }
        public string ColumnNumber { get; set; }
        public int? RowOrder { get; set; }
        public int? ColumnOrder { get; set; }
        public SeatType SeatTypeId { get; set; }
        public SeatStatus SeatStatusId { get; set; }
        public bool IsEnabled { get; set; }
        public long? EventTicketDetailId { get; set; }
        public long? SponsorId { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class MatchLayoutSectionSeatValidator : AbstractValidator<MatchLayoutSectionSeat>, IFILValidator
    {
        public MatchLayoutSectionSeatValidator()
        {
            RuleFor(s => s.SeatTag).NotEmpty().WithMessage("SeatTag is required");
            RuleFor(s => s.RowNumber).NotEmpty().WithMessage("RowNumber is required");
            RuleFor(s => s.ColumnNumber).NotEmpty().WithMessage("ColumnNumber is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}