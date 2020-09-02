using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.EventSchedule;

namespace FIL.Contracts.Queries.EventSchedule
{
    public class EditEventScheduleQuery : IQuery<EditEventScheduleQueryResult>
    {
        public long EventId { get; set; }
    }
}