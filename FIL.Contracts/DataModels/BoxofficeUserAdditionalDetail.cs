using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class BoxofficeUserAdditionalDetail : IId<long>, IAuditable, IAuditableDates
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long? ParentId { get; set; }
        public bool IsChildTicket { get; set; }
        public short IsETicket { get; set; }
        public bool IsSrCitizenTicket { get; set; }
        public short TicketLimit { get; set; }
        public short ChildTicketLimit { get; set; }
        public short ChildForPerson { get; set; }
        public short SrCitizenLimit { get; set; }
        public short SrCitizenPerson { get; set; }
        public long CityId { get; set; }
        public string Address { get; set; }
        public string ContactNumber { get; set; }
        public bool IsEnabled { get; set; }
        public int? CountryId { get; set; }
        public bool? IsBillExpressUser { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        public int? UserType { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }

        public class BoxofficeUserAdditionalDetailValidator : AbstractValidator<BoxofficeUserAdditionalDetail>, IKzValidator
        {
            public BoxofficeUserAdditionalDetailValidator()
            {
                RuleFor(s => s.UserId).NotEmpty().WithMessage("UserId is required");
                RuleFor(s => s.UserType).NotEmpty().WithMessage("UserType is required");
                RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
            }
        }
    }
}