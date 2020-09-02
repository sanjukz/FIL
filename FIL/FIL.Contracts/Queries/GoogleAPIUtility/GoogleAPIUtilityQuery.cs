using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.GoogleAPIUtility;

namespace FIL.Contracts.Queries.GoogleAPIUtility
{
    public class GoogleAPIUtilityQuery : IQuery<GoogleAPIUtilityQueryResult>
    {
        public string CityName { get; set; }
        public bool IsAll { get; set; }
    }
}