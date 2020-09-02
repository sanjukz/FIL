using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class EventFacebookReservation : IId<long>, IAuditable, IAuditableDates
    {
        public long Id { get; set; }
        public long EventId { get; set; }
        public long FbUserId { get; set; }
        public string FbUserName { get; set; }
        public string FbProfileUrl { get; set; }
        public string FbProfilePicUrl { get; set; }
        public string FbUserEmail { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class EventFacebookReservationValidator : AbstractValidator<EventFacebookReservation>, IKzValidator
    {
        public EventFacebookReservationValidator()
        {
            RuleFor(s => s.FbUserId).NotEmpty().WithMessage("FbUserId is required");
            RuleFor(s => s.FbUserName).NotEmpty().WithMessage("FbUserName is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}