using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.Country;

namespace FIL.Contracts.Queries.Country
{
    public class CountrySearchQuery : IQuery<CountrySearchQueryResult>
    {
        public string Name { get; set; }
    }
}