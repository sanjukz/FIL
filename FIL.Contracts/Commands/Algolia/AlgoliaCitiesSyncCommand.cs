using FIL.Contracts.DataModels;
using FIL.Contracts.Interfaces.Commands;
using System;
using System.Collections.Generic;

namespace FIL.Contracts.Commands.Algolia
{
    public class AlgoliaCitiesSyncCommand : ICommandWithResult<AlgoliaCitiesSyncCommandResult>
    {
        public List<Itinerary> AllCities { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class AlgoliaCitiesSyncCommandResult : ICommandResult
    {
        public long Id { get; set; }
        public bool IsSuccess { get; set; }
    }
}