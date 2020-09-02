namespace FIL.Contracts.Models
{
    public class EventAmenity
    {
        public int Id { get; set; }
        public long EventId { get; set; }
        public Amenities AmenityId { get; set; }
        public string Description { get; set; }
    }
}