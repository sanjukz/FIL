﻿using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class CountryRegionalOrganisationMapping : IId<int>, IAuditable, IAuditableDates
    {
        public int Id { get; set; }
        public int RegionalOrganisationId { get; set; }
        public int CountryId { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }
}