using FluentValidation;
using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class Role : IId<int>, IAuditable, IAuditableDates
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public Modules ModuleId { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class RoleValidator : AbstractValidator<Role>, IKzValidator
    {
        public RoleValidator()
        {
            RuleFor(s => s.RoleName).NotEmpty().WithMessage("Role name is required");
            RuleFor(s => s.ModuleId).NotEmpty().WithMessage("Module is required");
        }
    }
}