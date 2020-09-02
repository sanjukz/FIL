using FluentValidation;
using FIL.Contracts.Interfaces;
using System;

namespace FIL.Contracts.DataModels
{
    public class Activity : IId<int>
    {
        public int Id { get; set; }
        public string Module { get; set; }
        public string TableName { get; set; }
        public string Description { get; set; }
        public DateTime CreatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
    }

    public class ActivityValidator : AbstractValidator<Activity>, IKzValidator
    {
        public ActivityValidator()
        {
            RuleFor(s => s.Module).NotEmpty().WithMessage("Module is required");
            RuleFor(s => s.TableName).NotEmpty().WithMessage("TableName is required");
            RuleFor(s => s.Description).NotEmpty().WithMessage("Description is required");
        }
    }
}