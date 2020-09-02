using FIL.Contracts.DataModels;
using FIL.Contracts.Interfaces.Commands;
using FIL.Contracts.Models.Integrations.ExOz;
using System;
using System.Collections.Generic;

namespace FIL.Contracts.Commands.ExOz
{
    public class SaveExOzOperatorCommand : ICommandWithResult<SaveExOzOperatorCommandResult>
    {
        public List<ExOzOperatorResponse> OperatorList { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class SaveExOzOperatorCommandResult : ICommandResult
    {
        public long Id { get; set; }
        public List<ExOzOperator> OperatorList { get; set; }

        public List<ExOzProductResponse> ProductList { get; set; }
    }
}