using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Admin.ViewModels.EventCreation
{
    public class SaveEventDetailDataViewModel
    {
        public long Id { get; set; }
        public long EventId { get; set; }
        [Required]
        public string EventAltId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string VenueAltId { get; set; }
        [Required]
        public string StartDateTime { get; set; }
        [Required]
        public string EndDateTime { get; set; }
        [Required]
        public string MetaDetails { get; set; }
    }
}
