using FIL.Contracts.DataModels;
using FIL.Contracts.Interfaces.Commands;
using System;
using System.Collections.Generic;

namespace FIL.Contracts.Commands.ExOz
{
    public class SaveExOzCountryCommand : ICommandWithResult<SaveExOzCountryCommandResult>
    {
        public List<string> Names { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class SaveExOzCountryCommandResult : ICommandResult
    {
        public long Id { get; set; }
        public List<ExOzCountry> CountryList { get; set; }
    }
}