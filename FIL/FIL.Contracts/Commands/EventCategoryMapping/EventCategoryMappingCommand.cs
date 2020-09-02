namespace FIL.Contracts.Commands.EventCategoryMapping
{
    public class EventCategoryMappingCommand : BaseCommand
    {
        public int Eventid { get; set; }
        public int Categoryid { get; set; }
        public bool Isenabled { get; set; }
        public int Id { get; set; }
    }
}