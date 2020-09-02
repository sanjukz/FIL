using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults;
using System;

namespace FIL.Contracts.Queries.Venue
{
    public class VenueQuery : IQuery<VenueQueryResult>
    {
        public Guid CityAltId { get; set; }
        public long UserId { get; set; }
    }
}