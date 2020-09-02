using System;

namespace FIL.Contracts.Models
{
    public class PlaceWeekOpenDays
    {
        public long Id { get; set; }
        public Guid AltId { get; set; }
        public long EventId { get; set; }
        public long DayId { get; set; }
        public bool IsSameTime { get; set; }
        public bool IsEnabled { get; set; }
    }
}