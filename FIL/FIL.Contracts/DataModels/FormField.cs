using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class FormField : IId<int>, IAuditable
    {
        public int Id { get; set; }
        public string Caption { get; set; }
        public string Type { get; set; }
        public string ValidationScheme { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class FormFieldValidator : AbstractValidator<FormField>, IKzValidator
    {
        public FormFieldValidator()
        {
            RuleFor(s => s.Caption).NotEmpty().WithMessage("Caption is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}