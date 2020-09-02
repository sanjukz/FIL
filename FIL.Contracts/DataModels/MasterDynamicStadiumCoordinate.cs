﻿using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class MasterDynamicStadiumCoordinate : IId<int>, IAuditable, IAuditableDates
    {
        public int Id { get; set; }
        public Guid AltId { get; set; }
        public int MasterVenueLayoutId { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string SectionCoordinates { get; set; }
        public string SectionTextCoordinates { get; set; }
        public string CircleRectangleValue { get; set; }
        public string Styles { get; set; }
        public bool IsDisplay { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class MasterDynamicStadiumCoordinateValidator : AbstractValidator<MasterDynamicStadiumCoordinate>, IKzValidator
    {
        public MasterDynamicStadiumCoordinateValidator()
        {
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}