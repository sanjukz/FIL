using System;

namespace FIL.Contracts.Commands.CartTrack
{
    public class RemoveCartTrackCommand : BaseCommand
    {
        public string HubspotUTK { get; set; }
        public Guid UserAltId { get; set; }
    }
}