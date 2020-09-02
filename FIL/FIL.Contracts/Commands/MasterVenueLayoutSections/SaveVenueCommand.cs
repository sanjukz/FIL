using FIL.Contracts.Interfaces.Commands;
using System;

namespace FIL.Contracts.Commands.MasterVenueLayoutSections
{
    public class SaveVenueCommand : ICommandWithResult<SaveSaveVenueCommandResult>
    {
        public string City { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string VenueName { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string AddressLineOne { get; set; }
        public string AddressLineTwo { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class SaveSaveVenueCommandResult : ICommandResult
    {
        public long Id { get; set; }
        public string LayoutAltId { get; set; }
        public bool Success { get; set; }
        public bool IsExisting { get; set; }
    }
}