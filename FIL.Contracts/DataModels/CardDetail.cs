using FluentValidation;
using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class CardDetail : IId<int>, IAuditable, IAuditableDates
    {
        public int Id { get; set; }
        public string CardNumber { get; set; }
        public CardType? CardTypeId { get; set; }
        public string FirstSix { get; set; }
        public string Brand { get; set; }
        public string SubBrand { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string Bank { get; set; }
        public string Type { get; set; }
        public string Category { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class CardDetailValidator : AbstractValidator<CardDetail>, IKzValidator
    {
        public CardDetailValidator()
        {
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("CardNumber is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}