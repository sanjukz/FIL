using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class ReprintRequestDetail : IId<long>, IAuditable, IAuditableDates
    {
        public long Id { get; set; }
        public long RePrintRequestId { get; set; }
        public long MatchSeatTicketDetaildId { get; set; }
        public string BarcodeNumber { get; set; }
        public bool IsRePrinted { get; set; }
        public int RePrintCount { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        public bool? IsApproved { get; set; }
        public DateTime? ApprovedDateTime { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class ReprintRequestDetailValidator : AbstractValidator<ReprintRequestDetail>, IFILValidator
    {
        public ReprintRequestDetailValidator()
        {
            RuleFor(s => s.RePrintRequestId).NotEmpty().WithMessage("Reprint Request Id is required");
        }
    }
}