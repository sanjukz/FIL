using System;
using FluentValidation;
using FIL.Contracts.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class DonationDetail : IId<long>, IAuditable, IAuditableDates
    {
        public long Id { get; set; }
        public long EventId { get; set; }
        public decimal? DonationAmount1 { get; set; }
        public decimal? DonationAmount2 { get; set; }
        public decimal? DonationAmount3 { get; set; }
        public decimal? MinDonation { get; set; }
        public decimal? MaxDonation { get; set; }
        public bool IsFreeInput { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }
}
