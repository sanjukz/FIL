using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults;

namespace FIL.Contracts.Queries.City
{
    public class CitySearchQuery : IQuery<CitySearchQueryResult>
    {
        public string Name { get; set; }
    }
}