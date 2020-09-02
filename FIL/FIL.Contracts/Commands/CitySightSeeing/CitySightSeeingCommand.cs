using FIL.Contracts.Interfaces.Commands;
using System;
using System.Collections.Generic;

namespace FIL.Contracts.Commands.CitySightSeeing
{
    public class CitySightSeeingCommand : ICommandWithResult<CitySightSeeingCommandResult>
    {
        public int Step { get; set; }
        public FIL.Contracts.Models.CitySightSeeing.Location Location { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class CitySightSeeingCommandResult : ICommandResult
    {
        public long Id { get; set; }
        public bool Success { get; set; }
    }
}

public class GetAllLocationCommand : ICommandWithResult<GetAllLocationCommandResult>
{
    public Guid ModifiedBy { get; set; }
}

public class GetAllLocationCommandResult : ICommandResult
{
    public long Id { get; set; }
    public List<FIL.Contracts.DataModels.CitySightSeeingLocation> Locations { get; set; }
}