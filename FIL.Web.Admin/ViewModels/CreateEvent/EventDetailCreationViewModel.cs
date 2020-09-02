using FIL.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Kitms.Feel.ViewModels.PlaceCalendar
{
    public class EventDetailCreationViewModel
    {
        public PlaceCalendarRequestViewModel EventCalendar { get; set; }
        public List<Contracts.DataModels.EventHostMapping> EventHosts { get; set; }
        public string Title { get; set; }
        public int EventId { get; set; }
        public string EventCategoryId { get; set; }
        public string EventDescription { get; set; }
        public bool IsEdit { get; set; }
    }
}
