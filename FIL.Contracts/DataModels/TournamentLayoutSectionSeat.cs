﻿using FluentValidation;
using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class TournamentLayoutSectionSeat : IId<long>, IAuditable, IAuditableDates
    {
        public long Id { get; set; }
        public string SeatTag { get; set; }
        public int TournamentLayoutSectionId { get; set; }
        public string RowNumber { get; set; }
        public string ColumnNumber { get; set; }
        public int? RowOrder { get; set; }
        public int? ColumnOrder { get; set; }
        public SeatType SeatTypeId { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class TournamentLayoutSectionSeatValidator : AbstractValidator<TournamentLayoutSectionSeat>, IFILValidator
    {
        public TournamentLayoutSectionSeatValidator()
        {
            RuleFor(s => s.SeatTag).NotEmpty().WithMessage("SeatTag is required");
            RuleFor(s => s.RowNumber).NotEmpty().WithMessage("RowNumber is required");
            RuleFor(s => s.ColumnNumber).NotEmpty().WithMessage("ColumnNumber is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}