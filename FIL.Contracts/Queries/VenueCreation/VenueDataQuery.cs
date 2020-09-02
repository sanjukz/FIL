using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.VenueCreation;
using System;

namespace FIL.Contracts.Queries.VenueCreation
{
    public class VenueDataQuery : IQuery<VenueDataQueryResult>
    {
        public Guid VenueAltId { get; set; }
    }
}