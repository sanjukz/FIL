using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.CountryPlace;

namespace FIL.Contracts.Queries.CountryPlace
{
    public class CountryPlaceQuery : IQuery<CountryPlaceQueryResult>
    {
        public int ParentCategoryId { get; set; }
    }
}