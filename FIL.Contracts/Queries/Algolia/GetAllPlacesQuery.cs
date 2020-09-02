using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.Algolia;

namespace FIL.Contracts.Queries.Algolia
{
    public class GetAllPlacesQuery : IQuery<GetAllPlacesQueryResult>
    {
        public bool IsFeel { get; set; }
        public bool IsCities { get; set; }
        public int SkipIndex { get; set; }
        public int TakeIndex { get; set; }
    }
}