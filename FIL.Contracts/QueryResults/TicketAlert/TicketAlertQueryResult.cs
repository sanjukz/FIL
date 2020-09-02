using System.Collections.Generic;

namespace FIL.Contracts.QueryResults
{
    public class TicketAlertQueryResult
    {
        public FIL.Contracts.Models.Event Event { get; set; }
        public List<FIL.Contracts.Models.Country> Countries { get; set; }
        public List<FIL.Contracts.Models.Country> AllCountries { get; set; }
        public List<FIL.Contracts.Models.EventDetail> EventDetails { get; set; }
        public List<FIL.Contracts.Models.TicketAlertEventMapping> ticketAlertEventMappings { get; set; }
    }
}