using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class EventCategory : IId<int>, IAuditable
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public string DisplayName { get; set; }
        public string Slug { get; set; }
        public bool IsHomePage { get; set; }
        public int EventCategoryId { get; set; }
        public bool IsFeel { get; set; }
        public FIL.Contracts.Enums.MasterEventType MasterEventTypeId { get; set; }
        public int Order { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class EventCategoryValidator : AbstractValidator<EventCategory>, IFILValidator
    {
        public EventCategoryValidator()
        {
            RuleFor(s => s.Category).NotEmpty().WithMessage("Name is required");
        }
    }
}