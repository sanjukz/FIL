using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.Events;
using System;

namespace FIL.Contracts.Queries.Events
{
    public class EventsFeelSearchQuery : IQuery<EventsFeelQueryResult>
    {
        public bool IsFeel { get; set; }
        public string SearchString { get; set; }
        public long[] CategoryId { get; set; }
        public long[] SubCategoryId { get; set; }
        public Guid UserAltId { get; set; }
        public int RoleId { get; set; }
    }
}