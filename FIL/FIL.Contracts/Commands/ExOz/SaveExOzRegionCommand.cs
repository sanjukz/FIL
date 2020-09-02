using FIL.Contracts.DataModels;
using FIL.Contracts.Interfaces.Commands;
using FIL.Contracts.Models.Integrations.ExOz;
using System;
using System.Collections.Generic;

namespace FIL.Contracts.Commands.ExOz
{
    public class SaveExOzRegionCommand : ICommandWithResult<SaveExOzRegionCommandResult>
    {
        public List<ExOzRegionResponse> RegionList { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class SaveExOzRegionCommandResult : ICommandResult
    {
        public long Id { get; set; }
        public List<ExOzRegion> RegionList { get; set; }
        public OperatorList OperatorList { get; set; }
    }
}