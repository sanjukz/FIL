using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Admin.ViewModels.EventCreation
{
    public class EventCreationResponseDataViewModel
    {
        public bool Success { get; set; }
        public Guid AltId { get; set; }
        public long EventId { get; set; }
        public Guid VenueAltId { get; set; }
        public Guid PlaceAltId { get; set; }
        public bool IsAlreadyExists { get; set; }
    }
}
