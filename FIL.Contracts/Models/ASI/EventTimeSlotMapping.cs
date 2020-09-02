namespace FIL.Contracts.Models.ASI
{
    public class EventTimeSlotMapping
    {
        public long Id { get; set; }
        public long EventId { get; set; }
        public int TimeSlotId { get; set; }
        public string Name { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public bool IsEnabled { get; set; }
    }
}