using FIL.Contracts.DataModels;
using FIL.Contracts.Interfaces.Commands;
using FIL.Contracts.Models.Integrations.ExOz;
using System;
using System.Collections.Generic;

namespace FIL.Contracts.Commands.ExOz
{
    public class SaveExOzProductOptionCommand : ICommandWithResult<SaveExOzProductOptionCommandResult>
    {
        public List<ExOzProductOptionResponse> OptionList { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class SaveExOzProductOptionCommandResult : ICommandResult
    {
        public long Id { get; set; }
        public List<ExOzProductOption> OptionList { get; set; }
    }
}