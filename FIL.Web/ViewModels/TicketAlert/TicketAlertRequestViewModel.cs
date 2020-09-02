using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Feel.ViewModels.TicketAlert
{
    public class TicketAlertRequestViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneCode { get; set; }
        public string Email { get; set; }
        public string EventName { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsStreamingEvent { get; set; }
        public string Url { get; set;}
    }
}