using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class EventPartner : IId<int>, IAuditable
    {
        public int Id { get; set; }
        public Guid AltId { get; set; }
        public string PartnerName { get; set; }
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public int? Description { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class EventPartnerValidator : AbstractValidator<EventPartner>, IKzValidator
    {
        public EventPartnerValidator()
        {
            RuleFor(s => s.PartnerName).NotEmpty().WithMessage("PartnerName is required.");
            RuleFor(s => s.AccessKey).NotEmpty().WithMessage("AccessKey is required.");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}