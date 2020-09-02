using FIL.Contracts.DataModels;
using FIL.Contracts.Interfaces.Commands;
using FIL.Contracts.Models.Integrations.ExOz;
using System;
using System.Collections.Generic;

namespace FIL.Contracts.Commands.ExOz
{
    public class SaveExOzProductCommand : ICommandWithResult<SaveExOzProductCommandResult>
    {
        public List<ExOzProductResponse> ProductList { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class SaveExOzProductCommandResult : ICommandResult
    {
        public long Id { get; set; }
        public List<ExOzProduct> ProductList { get; set; }
    }
}