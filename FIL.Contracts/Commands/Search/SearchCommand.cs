using FIL.Contracts.DataModels;
using System;
using System.Collections.Generic;

namespace FIL.Contracts.Commands.Location
{
    public class SearchCommand : Contracts.Interfaces.Commands.ICommandWithResult<SearchCommandResult>
    {
        public List<Event> Events { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class SearchCommandResult : Contracts.Interfaces.Commands.ICommandResult
    {
        public long Id { get; set; }
        public Guid AltId { get; set; }
        public Guid ModifiedBy { get; set; }
    }
}