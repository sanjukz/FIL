using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.CreateEventV1
{
    public class EventRecurranceScheduleQueryResult
    {
        public bool Success { get; set; }
        public bool IsValidLink { get; set; }
        public bool IsDraft { get; set; }
        public List<FIL.Contracts.Models.CreateEventV1.EventRecurranceScheduleModel> EventRecurranceScheduleModel { get; set; }
    }
}