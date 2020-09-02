using FIL.Contracts.DataModels;
using FIL.Contracts.Interfaces.Commands;
using FIL.Contracts.Models.Integrations.ExOz;
using System;
using System.Collections.Generic;

namespace FIL.Contracts.Commands.ExOz
{
    public class SaveExOzStateCommand : ICommandWithResult<SaveExOzStateCommandResult>
    {
        public List<ExOzStateResponse> StateList { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class SaveExOzStateCommandResult : ICommandResult
    {
        public long Id { get; set; }
        public List<ExOzState> StateList { get; set; }
    }
}