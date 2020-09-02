using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults;
using System;

namespace FIL.Contracts.Queries.State
{
    public class StateQuery : IQuery<StateQueryResult>
    {
        public Guid CountryAltId { get; set; }
    }
}