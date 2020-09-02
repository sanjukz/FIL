using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults;

namespace FIL.Contracts.Queries.Venue
{
    public class VenueSearchQuery : IQuery<VenueSearchQueryResult>
    {
        public string Name { get; set; }
    }
}