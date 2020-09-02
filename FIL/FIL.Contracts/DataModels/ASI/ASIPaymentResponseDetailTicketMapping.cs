using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels.ASI
{
    public class ASIPaymentResponseDetailTicketMapping : IId<long>, IAuditable, IAuditableDates
    {
        public long Id { get; set; }
        public long ASIPaymentResponseDetailId { get; set; }
        public string VisitorId { get; set; }
        public DateTime Date { get; set; }
        public string TicketNo { get; set; }
        public string QrCode { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string IdentityType { get; set; }
        public string IdentityNo { get; set; }
        public string NationalityGroup { get; set; }
        public string NationalityCountry { get; set; }
        public string MonumentCode { get; set; }
        public string MonumentName { get; set; }
        public bool IsOptional { get; set; }
        public int MonumentTimeSlotId { get; set; }
        public bool IsAdult { get; set; }
        public decimal Amount { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }
}