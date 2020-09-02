using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults;
using System;

namespace FIL.Contracts.Queries.City
{
    public class CityQuery : IQuery<CityQueryResult>
    {
        public Guid StateAltId { get; set; }
    }
}