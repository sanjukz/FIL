using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.Events;

namespace FIL.Contracts.Queries.Events
{
    public class AmenitiesQuery : IQuery<AmenitiesQueryResult>
    {
        public int Id { get; set; }
        public string Amenity { get; set; }
    }
}