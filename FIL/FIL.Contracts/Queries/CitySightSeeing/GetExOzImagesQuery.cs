using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.CitySightSeeingLocation;

namespace FIL.Contracts.Queries.CitySightSeeingLocation
{
    public class GetExOzImagesQuery : IQuery<GetExOzImagesQueryResult>
    {
        public int TakeIndex { get; set; }
        public int SkipIndex { get; set; }
    }
}