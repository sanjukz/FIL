using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.CountryPlace;

namespace FIL.Contracts.Queries.CountryPlace
{
    public class StatePlaceQuery : IQuery<StatePlaceQueryResult>
    {
        public string CountryName { get; set; }
    }
}