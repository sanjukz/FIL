using System;

namespace FIL.Contracts.Commands.GoogleAPIUtility
{
    public class GoogleAPIUtilityCommand : Contracts.Interfaces.Commands.ICommandWithResult<GoogleAPIUtilityCommandResult>
    {
        public int VenueId { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class GoogleAPIUtilityCommandResult : Contracts.Interfaces.Commands.ICommandResult
    {
        public long Id { get; set; }
        public bool Success { get; set; }
    }
}