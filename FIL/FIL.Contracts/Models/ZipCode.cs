using System;

namespace FIL.Contracts.Models
{
    public class Zipcode
    {
        public Guid AltId { get; set; }
        public string Postalcode { get; set; }
        public string Region { get; set; }
        public int CityId { get; set; }
    }
}