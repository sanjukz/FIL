﻿using FluentValidation;
using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class ReprintRequest : IId<long>, IAuditable, IAuditableDates
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long TransactionId { get; set; }
        public DateTime RequestDateTime { get; set; }
        public string Remarks { get; set; }
        public bool IsApproved { get; set; }
        public long? ApprovedBy { get; set; }
        public DateTime? ApprovedDateTime { get; set; }
        public Modules ModuleId { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        public Guid? AltId { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class ReprintRequestValidator : AbstractValidator<ReprintRequest>, IFILValidator
    {
        public ReprintRequestValidator()
        {
            RuleFor(s => s.UserId).NotEmpty().WithMessage("UserId is required");
            RuleFor(s => s.TransactionId).NotEmpty().WithMessage("TransactionId is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}