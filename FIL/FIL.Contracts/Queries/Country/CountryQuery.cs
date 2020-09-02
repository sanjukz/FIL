using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults;
using System;

namespace FIL.Contracts.Queries.Country
{
    public class CountryQuery : IQuery<CountryQueryResult>
    {
        public Guid AltId { get; set; }
    }
}