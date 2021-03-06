﻿using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class EventFormField
    {
        public long Id { get; set; }
        public long EventId { get; set; }
        public int FormFieldId { get; set; }
        public string Value { get; set; }
        public bool? IsMandatory { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class EventFormFieldValidator : AbstractValidator<EventFormField>, IFILValidator
    {
        public EventFormFieldValidator()
        {
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}