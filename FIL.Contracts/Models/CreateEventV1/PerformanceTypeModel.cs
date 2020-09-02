namespace FIL.Contracts.Models.CreateEventV1
{
    public class PerformanceTypeModel
    {
        public int Id { get; set; }
        public long EventId { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsVideoUploaded { get; set; }
        public FIL.Contracts.Enums.PerformanceType PerformanceTypeId { get; set; }
        public FIL.Contracts.Enums.OnlineEventTypes OnlineEventTypeId { get; set; }
    }
}