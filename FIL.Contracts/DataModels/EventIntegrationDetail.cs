using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class EventIntegrationDetail : IId<int>, IAuditable
    {
        public int Id { get; set; }
        public Guid AltId { get; set; }
        public long EventDetailId { get; set; }
        public int PartnerId { get; set; }
        public string AccessKey { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class EventIntegrationDetailValidator : AbstractValidator<EventIntegrationDetail>, IKzValidator
    {
        public EventIntegrationDetailValidator()
        {
            RuleFor(s => s.AccessKey).NotEmpty().WithMessage("AccessKey is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}