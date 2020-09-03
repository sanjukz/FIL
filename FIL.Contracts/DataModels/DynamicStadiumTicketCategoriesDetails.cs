using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class DynamicStadiumTicketCategoriesDetails : IId<int>, IAuditable, IAuditableDates
    {
        public int Id { get; set; }
        public Guid AltId { get; set; }
        public int DynamicStadiumCoordinateId { get; set; }
        public int TicketCategoryId { get; set; }
        public int FillingFastPercentage { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class DynamicStadiumTicketCategoriesDetailsValidator : AbstractValidator<DynamicStadiumTicketCategoriesDetails>, IFILValidator
    {
        public DynamicStadiumTicketCategoriesDetailsValidator()
        {
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}