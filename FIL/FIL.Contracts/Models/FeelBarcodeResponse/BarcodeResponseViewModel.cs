using System.Collections.Generic;

namespace FIL.Contracts.Models.FeelBarcodeResponse
{
    public class TicketDetail
    {
        public string ticket_name { get; set; }
        public string ticket_type { get; set; }
        public string ticket_code { get; set; }
    }

    public class BookingDetail
    {
        public string ticket_id { get; set; }
        public string venue_name { get; set; }
        public string code_type { get; set; }
        public string group_code { get; set; }
        public IList<TicketDetail> ticket_details { get; set; }
    }

    public class Data
    {
        public string distributor_reference { get; set; }
        public string booking_reference { get; set; }
        public string booking_status { get; set; }
        public IList<BookingDetail> booking_details { get; set; }
    }

    public class BookingResponse
    {
        public string response_type { get; set; }
        public Data data { get; set; }
    }
}