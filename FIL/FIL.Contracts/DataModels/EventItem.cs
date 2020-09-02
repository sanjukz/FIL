using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class EventItem : IId<int>, IAuditable, IAuditableDates
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? EventDate { get; set; }
        public int MinPrice { get; set; }
        public int MaxPrice { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Type { get; set; }
        public string Currency { get; set; }
        public string ImagePath { get; set; }
        public string RSVPImage { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }
}