namespace FIL.Contracts.Models
{
    public class EventVenueMappingTime
    {
        public int Id { get; set; }
        public int EventVenueMappingId { get; set; }
        public string PickupTime { get; set; }
        public string PickupLocation { get; set; }
        public string ReturnTime { get; set; }
        public string ReturnLocation { get; set; }
        public int? JourneyType { get; set; }
        public string WaitingTime { get; set; }
    }
}