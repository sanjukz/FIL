using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.Events;

namespace FIL.Contracts.Queries.Events
{
    public class EventCategoryQuery : IQuery<EventCategoryQueryResult>
    {
        public int Id { get; set; }
        public string Slug { get; set; }
    }
}