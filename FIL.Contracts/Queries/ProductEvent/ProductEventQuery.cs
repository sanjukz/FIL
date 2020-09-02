using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.ProductEvent;

namespace FIL.Contracts.Queries.ProductEvent
{
    public class ProductEventQuery : IQuery<ProductEventQueryResult>
    {
        public bool IsFeel { get; set; }
    }
}