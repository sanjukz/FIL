using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.Country;
using System;

namespace FIL.Contracts.Queries.Country
{
    public class CountryGetQuery : IQuery<CountryGetQueryResult>
    {
        public Guid AltId { get; set; }
    }
}