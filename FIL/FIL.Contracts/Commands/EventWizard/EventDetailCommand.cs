using System;

namespace FIL.Contracts.Commands.EventWizard
{
    public class EventDetailCommand : BaseCommand
    {
        public int VenueId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public int? GroupId { get; set; }
    }
}