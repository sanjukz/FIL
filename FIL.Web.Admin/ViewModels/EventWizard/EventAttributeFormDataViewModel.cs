using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Kitms.Feel.ViewModels.EventWizard
{
    public class EventAttributeFormDataViewModel
    {        
        public short? MatchNo { get; set; }     
        public short? MatchDay { get; set; }
        [Required]
        public string GateOpenTime { get; set; }
        [Required]
        public string TimeZone { get; set; }
        [Required]
        public string TimeZoneAbbreviation { get; set; }
        [Required]
        public string TicketHtml { get; set; }
    }
}
