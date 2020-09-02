using FluentValidation;
using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class FeelUserAdditionalDetail : IId<int>, IAuditable
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public bool OptedForMailer { get; set; }
        public int OptOutStatusId { get; set; }
        public string SocialLoginId { get; set; }
        public int SignUpMethodId { get; set; }
        public string BirthDate { get; set; }
        public Gender? Gender { get; set; }
        public int ResidentId { get; set; }
        public int CitizenId { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class FeelUserAdditionalDetailValidator : AbstractValidator<FeelUserAdditionalDetail>, IKzValidator
    {
        public FeelUserAdditionalDetailValidator()
        {
            RuleFor(s => s.BirthDate).NotEmpty().WithMessage("BirthDate is required");
            RuleFor(s => s.Gender).NotEmpty().WithMessage("Gender is required");
        }
    }
}