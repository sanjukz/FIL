using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class EventTokenMapping : IId<int>, IAuditable
    {
        public int Id { get; set; }
        public int TokenId { get; set; }
        public long EventId { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class EventTokenMappingValidator : AbstractValidator<EventTokenMapping>, IKzValidator
    {
        public EventTokenMappingValidator()
        {
            RuleFor(s => s.TokenId).NotEmpty().WithMessage("Tokenid is required");
            RuleFor(s => s.EventId).NotEmpty().WithMessage("EventId is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}