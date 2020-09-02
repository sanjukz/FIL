using FIL.Contracts.Interfaces.Commands;
using FIL.Contracts.Models;
using System;
using System.Collections.Generic;

namespace FIL.Contracts.Commands.MasterVenueLayoutSections
{
    public class CreateSeatLayoutCommand : ICommandWithResult<CreateSeatLayoutCommandResult>
    {
        public List<MasterVenueLayoutSectionSeat> MasterVenueLayoutSectionSeatDetails { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class CreateSeatLayoutCommandResult : ICommandResult
    {
        public bool Success { get; set; }
        public long Id { get; set; }
        public bool IsExisting { get; set; }
        public Guid ModifiedBy { get; set; }
    }
}