using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults;
using System;

namespace FIL.Contracts.Queries.State
{
    public class StateListQuery : IQuery<StateListQueryResult>
    {
        public Guid CountryAltId { get; set; }
    }
}