using System;

namespace FIL.Contracts.Models
{
    public class CountryContentMapping
    {
        public long Id { get; set; }
        public int CountryId { get; set; }
        public string Heading { get; set; }
        public string SubHeading { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
    }
}