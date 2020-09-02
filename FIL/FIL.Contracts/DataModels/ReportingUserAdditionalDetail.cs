using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class ReportingUserAdditionalDetail : IId<long>, IAuditable
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public bool IsCredentialsMailed { get; set; }
        public string ProfilePic { get; set; }
        public string ClientLogo { get; set; }
        public bool IsEnabled { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedUtc { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedUtc { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class ReportingUserAdditionalDetailValidator : AbstractValidator<ReportingUserAdditionalDetail>, IKzValidator
    {
        public ReportingUserAdditionalDetailValidator()
        {
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}