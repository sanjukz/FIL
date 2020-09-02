using System.Collections.Generic;

namespace FIL.Contracts.Commands.UserCreation
{
    public class EventAssignCommand : BaseCommand
    {
        public string UserAltId { get; set; }
        public List<long> EventIds { get; set; }
    }
}