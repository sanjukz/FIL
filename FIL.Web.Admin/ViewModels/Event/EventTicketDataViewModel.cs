using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Admin.ViewModels.Event
{
    public class EventTicketDataViewModel
    {
        [Required]
        public List<EventDataViewModel> Venues { get; set; }
    }
}
