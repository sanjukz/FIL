using System;

namespace FIL.Contracts.Commands.EventCreation
{
    public class SaveAmenityCommand : Contracts.Interfaces.Commands.ICommandWithResult<SaveAmenityCommandResult>
    {
        public int Id { get; set; }
        public string Amenity { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class SaveAmenityCommandResult : Contracts.Interfaces.Commands.ICommandResult
    {
        public long Id { get; set; }
    }
}