namespace FIL.Contracts.Commands.EventCreation
{
    public class EventAmenityCommand : BaseCommand
    {
        public int Id { get; set; }
        public long EventId { get; set; }
        public int AmenityId { get; set; }
        public bool IsEnabled { get; set; }
        public string Description { get; set; }
    }
}