using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class ExOzPurchaser : IId<long>, IAuditable
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
        public string StateName { get; set; }
        public string Suburb { get; set; }
        public string Postcode { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class ExOzPurchaserValidator : AbstractValidator<ExOzPurchaser>, IKzValidator
    {
        public ExOzPurchaserValidator()
        {
            RuleFor(s => s.FirstName).NotEmpty().WithMessage("FirstName is required");
            RuleFor(s => s.LastName).NotEmpty().WithMessage("LastName is required");
            RuleFor(s => s.Email).NotEmpty().WithMessage("Email is required");
        }
    }
}