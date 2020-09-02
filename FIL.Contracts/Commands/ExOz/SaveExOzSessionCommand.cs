using FIL.Contracts.DataModels;
using FIL.Contracts.Interfaces.Commands;
using FIL.Contracts.Models.Integrations.ExOz;
using System;
using System.Collections.Generic;

namespace FIL.Contracts.Commands.ExOz
{
    public class SaveExOzSessionCommand : ICommandWithResult<SaveExOzSessionCommandResult>
    {
        public List<ExOzSessionResponse> SessionList { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class SaveExOzSessionCommandResult : ICommandResult
    {
        public long Id { get; set; }
        public List<ExOzProductSession> SessionList { get; set; }
    }
}