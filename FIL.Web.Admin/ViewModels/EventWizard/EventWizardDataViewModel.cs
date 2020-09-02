using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Kitms.Feel.ViewModels.EventWizard
{
    public class EventWizardDataViewModel
    {
        [Required]
        public EventFormDataViewModel EventForm { get; set; }
        [Required]
        public EventDetailFormDataViewModel EventDetail { get; set; }
        [Required]
        public EventTicketAttributeFormDataViewModel EventTicketAttribute { get; set; }
        [Required]
        public EventAttributeFormDataViewModel EventAttribute { get; set; }
    }
}
