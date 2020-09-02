using FIL.Contracts.Interfaces.Commands;
using System;

namespace FIL.Contracts.Commands.TournamentLayout
{
    public class EditStandDataCommand : ICommandWithResult<EditStandDataCommandResult>
    {
        public int SectionId { get; set; }
        public string StandName { get; set; }
        public string EntryGateAltId { get; set; }
        public int EventId { get; set; }
        public int MasterVenueLayoutId { get; set; }
        public int Capacity { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class EditStandDataCommandResult : ICommandResult
    {
        public long Id { get; set; }
        public bool Success { get; set; }
        public bool IsNotCapacityAvailable { get; set; }
        public int AvailableCapacity { get; set; }
    }
}