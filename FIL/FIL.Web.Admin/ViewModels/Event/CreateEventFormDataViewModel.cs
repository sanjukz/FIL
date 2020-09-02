using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Kitms.Feel.ViewModels.Event
{
    public class CreateEventFormDataViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Searchkeyword { get; set; }
        public string HotTicketImage { get; set; }
        public string EventhomeImage { get; set; }
        public string AboutEventImage { get; set; }
        public string TileImage { get; set; }
        public string TermsandConditions { get; set; }
        public Boolean IsPublish { get; set; }
    }
}
