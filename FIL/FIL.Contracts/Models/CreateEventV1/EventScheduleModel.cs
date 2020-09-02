using System;

namespace FIL.Contracts.Models.CreateEventV1
{
    public class EventScheduleModel
    {
        public long EventId { get; set; }
        public long EventDetailId { get; set; }
        public int VenueId { get; set; }
        public int DayId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public DateTime LocalStartDateTime { get; set; }
        public DateTime LocalEndDateTime { get; set; }
        public FIL.Contracts.Enums.EventFrequencyType EventFrequencyType { get; set; }
        public string LocalStartTime { get; set; }
        public string LocalEndTime { get; set; }
        public string TimeZoneAbbrivation { get; set; }
        public string TimeZoneOffset { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsCreate { get; set; }
    }

    public class EventRecurranceScheduleModel
    {
        public long EventScheduleId { get; set; }
        public long ScheduleDetailId { get; set; }
        public string DayIds { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public DateTime LocalStartDateTime { get; set; }
        public DateTime LocalEndDateTime { get; set; }
        public DateTime EventScheduleStartDateTime { get; set; }
        public DateTime EventScheduleEndDateTime { get; set; }
        public string LocalStartTime { get; set; }
        public string LocalEndTime { get; set; }
        public string LocalStartDateString { get; set; }
        public string LocalEndDateString { get; set; }
        public string LocalEventScheduleStartDateTimeString { get; set; }
        public string LocalEventScheduleEndDateTimeString { get; set; }
        public bool IsEnabled { get; set; }
    }
}