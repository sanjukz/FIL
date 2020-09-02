using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.Country;

namespace FIL.Contracts.Queries.Country
{
    public class CountrySearchInfoQuery : IQuery<CountrySearchInfoQueryResult>
    {
        public string Name { get; set; }
    }
}