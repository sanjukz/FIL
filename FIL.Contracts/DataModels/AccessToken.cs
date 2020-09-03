using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class AccessToken : IId<int>, IAuditable
    {
        public int Id { get; set; }
        public Guid AltId { get; set; }
        public string Token { get; set; }
        public int PartnerId { get; set; }
        public string TokenType { get; set; }
        public string ExpiresIn { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class AccessTokenValidator : AbstractValidator<AccessToken>, IFILValidator
    {
        public AccessTokenValidator()
        {
            RuleFor(s => s.Token).NotEmpty().WithMessage("Token is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}