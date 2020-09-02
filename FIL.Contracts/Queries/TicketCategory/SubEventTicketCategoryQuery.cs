using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.TicketCategories;

namespace FIL.Contracts.Queries.TicketCategory
{
    public class SubEventTicketCategoryQuery : IQuery<SubEventTicketCategoryQueryResult>
    {
        public long EventDetailId { get; set; }
    }
}