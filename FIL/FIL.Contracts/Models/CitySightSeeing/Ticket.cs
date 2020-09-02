using System.Collections.Generic;

namespace FIL.Contracts.Models.CitySightSeeing
{
    public class Ticket
    {
        public string ticket_id { get; set; }
        public string ticket_title { get; set; }
        public string venue_name { get; set; }
        public string txt_language { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }
        public string currency { get; set; }
        public List<Location> locations { get; set; }
    }

    public class TicketData
    {
        public List<Ticket> tickets { get; set; }
    }

    public class TicketList
    {
        public string response_type { get; set; }
        public TicketData data { get; set; }
    }
}