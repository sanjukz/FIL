using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class ZoomMeeting : IId<long>, IAuditable, IAuditableDates
    {
        public long Id { get; set; }
        public long MeetingNumber { get; set; }
        public string MeetingUuid { get; set; }
        public long EventId { get; set; }
        public string HostUserId { get; set; }
        public int DurationMinutes { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class ZoomMeetingValidator : AbstractValidator<ZoomMeeting>, IKzValidator
    {
        public ZoomMeetingValidator()
        {
            RuleFor(s => s.MeetingNumber).NotEmpty().WithMessage("Meeting Number is required");
            RuleFor(s => s.EventId).NotEmpty().WithMessage("Event Id required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}