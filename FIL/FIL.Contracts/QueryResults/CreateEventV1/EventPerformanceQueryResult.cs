namespace FIL.Contracts.QueryResults.CreateEventV1
{
    public class EventPerformanceQueryResult
    {
        public bool Success { get; set; }
        public long EventId { get; set; }
        public System.Guid? EventAltId { get; set; }
        public string OnlineEventType { get; set; }
        public FIL.Contracts.Enums.EventFrequencyType EventFrequencyType { get; set; }
        public FIL.Contracts.Models.CreateEventV1.PerformanceTypeModel PerformanceTypeModel { get; set; }
    }
}