using FIL.Contracts.Interfaces.Commands;
using FIL.Contracts.Models.TournamentLayouts;
using System;
using System.Collections.Generic;

namespace FIL.Contracts.Commands.TournamentLayout
{
    public class SaveFeeDataCommand : ICommandWithResult<SaveFeeDataCommandResult>
    {
        public long EventId { get; set; }
        public Guid ModifiedBy { get; set; }
        public IEnumerable<EventFeeTypeMappings> FeeDetails { get; set; }
        public bool isInsert { get; set; }
    }

    public class SaveFeeDataCommandResult : ICommandResult
    {
        public long Id { get; set; }
        public bool Success { get; set; }
    }
}