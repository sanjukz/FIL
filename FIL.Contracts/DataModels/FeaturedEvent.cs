﻿using FluentValidation;
using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class FeaturedEvent : IId<int>, IAuditable, IAuditableDates
    {
        public int Id { get; set; }
        public long EventId { get; set; }
        public short SortOrder { get; set; }
        public Site SiteId { get; set; }
        public bool? IsAllowedInFooter { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class FeaturedEventValidator : AbstractValidator<FeaturedEvent>, IKzValidator
    {
        public FeaturedEventValidator()
        {
            RuleFor(s => s.SortOrder).NotEmpty().WithMessage("SortOrder is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}