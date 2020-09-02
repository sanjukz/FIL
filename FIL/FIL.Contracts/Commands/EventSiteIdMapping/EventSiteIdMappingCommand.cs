using FIL.Contracts.Enums;

namespace FIL.Contracts.Commands.EventSiteIdMapping
{
    public class EventSiteIdMappingCommand : BaseCommand
    {
        public int Id { get; set; }
        public long EventId { get; set; }
        public short SortOrder { get; set; }
        public Site SiteId { get; set; }
        public bool IsEnabled { get; set; }
    }
}