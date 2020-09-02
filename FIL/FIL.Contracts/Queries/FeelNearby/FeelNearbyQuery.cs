using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults;

namespace FIL.Contracts.Queries
{
    public class FeelNearbyQuery : IQuery<FeelNearbyQueryResult>
    {
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public decimal Distance { get; set; }
    }
}