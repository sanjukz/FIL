using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults;
using System;

namespace FIL.Contracts.Queries.Zipcode
{
    public class ZipcodeQuery : IQuery<ZipcodeQueryResult>
    {
        public Guid CityAltId { get; set; }
    }
}