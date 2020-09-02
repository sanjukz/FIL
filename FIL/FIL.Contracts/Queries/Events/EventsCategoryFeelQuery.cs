using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.Events;

namespace FIL.Contracts.Queries.Events
{
    public class EventsCategoryFeelQuery : IQuery<EventsCategoryFeelQueryResult>
    {
        public bool IsEnabled { get; set; }
    }
}