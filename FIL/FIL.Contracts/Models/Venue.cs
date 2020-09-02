using System;

namespace FIL.Contracts.Models
{
    public class Venue
    {
        public int Id { get; set; }
        public Guid AltId { get; set; }
        public string Name { get; set; }
        public string AddressLineOne { get; set; }
        public string AddressLineTwo { get; set; }
        public int CityId { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public bool? HasImages { get; set; }
        public string Prefix { get; set; }
    }
}