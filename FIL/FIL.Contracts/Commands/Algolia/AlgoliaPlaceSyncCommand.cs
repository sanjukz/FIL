using FIL.Contracts.Interfaces.Commands;
using FIL.Contracts.Models;
using System;
using System.Collections.Generic;

namespace FIL.Contracts.Commands.Algolia
{
    public class AlgoliaPlaceSyncCommand : ICommandWithResult<AlgoliaPlaceSyncCommandResult>
    {
        public List<PlaceDetail> AllPlaces { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class AlgoliaPlaceSyncCommandResult : ICommandResult
    {
        public long Id { get; set; }
        public bool IsSuccess { get; set; }
    }
}