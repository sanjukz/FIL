using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.EventCreation;

namespace FIL.Contracts.Queries.EventCreation
{
    public class EventCreationCategoryQuery : IQuery<EventCreationCategoryQueryResult>
    {
        public int Id { get; set; }
        public string Slug { get; set; }
    }
}