using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class UserTokenMapping : IId<int>, IAuditable
    {
        public int Id { get; set; }
        public int TokenId { get; set; }
        public long UserId { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class UserTokenMappingValidator : AbstractValidator<UserTokenMapping>, IKzValidator
    {
        public UserTokenMappingValidator()
        {
            RuleFor(s => s.TokenId).NotEmpty().WithMessage("Tokenid is required");
            RuleFor(s => s.UserId).NotEmpty().WithMessage("UserId is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}