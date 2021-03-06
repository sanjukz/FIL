﻿using FluentValidation;
using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class Feature : IId<int>, IAuditable, IAuditableDates
    {
        public int Id { get; set; }
        public string FeatureName { get; set; }
        public Modules ModuleId { get; set; }
        public int ParentFeatureId { get; set; }
        public string RedirectUrl { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class FeatureValidator : AbstractValidator<Feature>, IFILValidator
    {
        public FeatureValidator()
        {
            RuleFor(s => s.FeatureName).NotEmpty().WithMessage("Feature name is required");
            RuleFor(s => s.ModuleId).NotEmpty().WithMessage("Module is required");
        }
    }
}