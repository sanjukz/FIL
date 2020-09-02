using System;

namespace FIL.Contracts.Commands.CreateEventV1
{
    public class EventRecurranceCommand : Contracts.Interfaces.Commands.ICommandWithResult<EventRecurranceCommandResult>
    {
        public long EventId { get; set; }
        public long EventScheduleId { get; set; }
        public long ScheduleDetailId { get; set; }
        public short CurrentStep { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public FIL.Contracts.Enums.OccuranceType OccuranceType { get; set; }
        public FIL.Contracts.Enums.EventFrequencyType EventFrequencyType { get; set; }
        public string DayIds { get; set; }
        public string LocalStartTime { get; set; }
        public string LocalEndTime { get; set; }
        public int OccuranceCount { get; set; }
        public string TimeZoneOffSet { get; set; }
        public string TimeZoneAbbrivation { get; set; }
        public ActionType ActionType { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public enum ActionType
    {
        BulkInsert = 1,
        BulkReschedule,
        BulkDelete,
        SingleReschedule,
        SingleDelete
    }

    public class EventRecurranceCommandResult : Contracts.Interfaces.Commands.ICommandResult
    {
        public long Id { get; set; }
        public bool Success { get; set; }
        public short CurrentStep { get; set; }
        public string CompletedStep { get; set; }
    }
}