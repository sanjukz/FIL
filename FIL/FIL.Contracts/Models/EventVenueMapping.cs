namespace FIL.Contracts.Models
{
    public class EventVenueMapping
    {
        public int Id { get; set; }
        public long EventId { get; set; }
        public int VenueId { get; set; }
    }
}