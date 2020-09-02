using FluentValidation;
using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class RASVTicketTypeMapping : IId<int>, IAuditable, IAuditableDates
    {
        public int Id { get; set; }
        public long EventDetailId { get; set; }
        public RASVTicketType RASVTicketTypeId { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class ExhibitValidator : AbstractValidator<RASVTicketTypeMapping>, IKzValidator
    {
        public ExhibitValidator()
        {
        }
    }
}