namespace FIL.Contracts.QueryResults.CreateEventV1
{
    public class EventDetailsQueryResult
    {
        public bool Success { get; set; }

        public bool IsValidLink { get; set; }
        public bool IsDraft { get; set; }
        public FIL.Contracts.Models.CreateEventV1.EventDetailModel EventDetail { get; set; }
    }
}