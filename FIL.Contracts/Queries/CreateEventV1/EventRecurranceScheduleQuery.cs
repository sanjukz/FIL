using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.CreateEventV1;
using System;

namespace FIL.Contracts.Queries.CreateEventV1
{
    public class EventRecurranceScheduleQuery : IQuery<EventRecurranceScheduleQueryResult>
    {
        public long EventId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}