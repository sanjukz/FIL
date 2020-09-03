using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Admin.ViewModels.Event
{
    public class AllocationManagerVenueDataViewModel
    {
        [Required]
        public string Stand { get; set; }
        [Required]
        public string Block { get; set; }
        [Required]
        public string Level { get; set; }
        [Required]
        public string TicketsAvailable { get; set; }
        [Required]
        public string TBD { get; set; }
    }
}
