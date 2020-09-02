using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.Category;
using System.Collections.Generic;

namespace FIL.Contracts.Queries.Category
{
    public class CategoriesByLocationQuery : IQuery<CategoriesByLocationQueryResult>
    {
        public List<int> CityIds { get; set; }
    }
}