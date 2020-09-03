using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Admin.ViewModels.EventCreation
{
    public class EventDetailResponseViewModel
    {
        public long Id { get; set; }
        public int VenueId { get; set; }
        public bool Success { get; set; }
    }
}
