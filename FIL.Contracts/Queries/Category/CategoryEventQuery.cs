using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.Category;

namespace FIL.Contracts.Queries.Category
{
    public class CategoryEventQuery : IQuery<CategoryEventQueryResult>
    {
        public int EventCategoryId { get; set; }
    }
}