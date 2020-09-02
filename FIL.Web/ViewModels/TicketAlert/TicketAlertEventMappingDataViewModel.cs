using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Feel.ViewModels.TicketAlert
{
    public class TicketAlertEventMappingDataViewModel
    {
        public FIL.Contracts.Models.Event Event { get; set; }
        public List<FIL.Contracts.Models.Country> Countries { get; set; }
        public List<FIL.Contracts.Models.Country> AllCountriesModel { get; set; }
        public List<FIL.Contracts.Models.EventDetail> EventDetails { get; set; }
        public List<FIL.Contracts.Models.TicketAlertEventMapping> TicketAlertEventMappings { get; set; }
    }
}
