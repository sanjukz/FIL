using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.CitySearch;

namespace FIL.Contracts.Queries.CitySearch
{
    public class CitySearchQuery : IQuery<CitySearchQueryResult>
    {
        public string Name { get; set; }
    }
}