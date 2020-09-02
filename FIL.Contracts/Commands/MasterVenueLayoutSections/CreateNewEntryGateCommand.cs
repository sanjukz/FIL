using FIL.Contracts.Interfaces.Commands;
using System;

namespace FIL.Contracts.Commands.MasterVenueLayoutSections
{
    public class CreateNewEntryGateCommand : ICommandWithResult<CreateNewEntryGateCommandResult>
    {
        public string NewEntryGatename { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class CreateNewEntryGateCommandResult : ICommandResult
    {
        public long Id { get; set; }
        public bool Success { get; set; }
        public string AltId { get; set; }
        public bool IsExisting { get; set; }
    }
}