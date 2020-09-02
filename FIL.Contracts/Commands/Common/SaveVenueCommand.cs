using System;

namespace FIL.Contracts.Commands.Common
{
    public class SaveVenueCommand : BaseCommand
    {
        public string Name { get; set; }
        public string AddressLineOne { get; set; }
        public string AddressLineTwo { get; set; }
        public Guid CityAltId { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public bool? HasImages { get; set; }
        public string Prefix { get; set; }
    }
}