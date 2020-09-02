using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.EventSchedule;
using System;

namespace FIL.Contracts.Queries.EventSchedule
{
    public class EventScheduleQuery : IQuery<EventScheduleQueryResult>
    {
        public Guid AltId { get; set; }
        public int Venueid { get; set; }
    }
}