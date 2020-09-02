using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Feel.ViewModels.TicketAlert
{
    public class TicketAlertUserMappingRequestDataViewModel
    {
        public List<string> CountriesAltIds { get; set; }
        public List<int> TicketAlertEventMapping { get; set; }
        [Required]
        public Guid EventAltId { get; set; }
        [Required]
        public string EventName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneCode { get; set; }
        [Required]
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int TourAndTavelPackage { get; set; }
        public int NoOfTickets { get; set; }
        public bool? isStreamingEvent { get; set;}
    }
    public class CountryData
    {
        public string countryAltId { get; set; }
    }
}
