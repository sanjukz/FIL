using System;

namespace FIL.Contracts.Commands.BoxOffice
{
    public class UserEventAssignCommand : BaseCommand
    {
        public Guid UserAltId { get; set; }
        public Guid EventAltId { get; set; }
        public Guid VenueAltId { get; set; }
    }
}