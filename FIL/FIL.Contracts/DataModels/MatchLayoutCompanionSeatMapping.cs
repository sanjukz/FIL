﻿using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class MatchLayoutCompanionSeatMapping : IId<int>, IAuditable, IAuditableDates
    {
        public int Id { get; set; }
        public long WheelChairSeatId { get; set; }
        public long CompanionSeatId { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class MatchLayoutCompanionSeatMappingValidator : AbstractValidator<MatchLayoutCompanionSeatMapping>, IKzValidator
    {
        public MatchLayoutCompanionSeatMappingValidator()
        {
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}