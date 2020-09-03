using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Admin.ViewModels.EventWizard
{
    public class EventDetailFormDataViewModel
    {
        [Required]
        public int VenueId { get; set; }
        [Required]
        public DateTime StartDateTime { get; set; }
        [Required]
        public DateTime EndDateTime { get; set; }        
        public int? GroupId { get; set; }
    }
}
