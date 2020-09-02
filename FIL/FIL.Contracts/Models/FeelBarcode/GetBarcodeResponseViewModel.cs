using System.Collections.Generic;

namespace FIL.Contracts.Models.FeelBarcode
{
    public class GetBarcodeResponseViewModel
    {
        public RootObject rootObject { get; set; }
    }

    public class BookingDetail
    {
        public string ticket_type { get; set; }
        public int count { get; set; }
        public List<object> extra_options { get; set; }
    }

    public class BookingType
    {
        public string ticket_id { get; set; }
        public string from_date_time { get; set; }
        public string to_date_time { get; set; }
        public List<BookingDetail> booking_details { get; set; }
    }

    public class Address
    {
        public string street { get; set; }
        public string postal_code { get; set; }
        public string city { get; set; }
    }

    public class Contact
    {
        public Address address { get; set; }
        public string phonenumber { get; set; }
    }

    public class Data
    {
        public string distributor_id { get; set; }
        public string currency { get; set; }
        public BookingType booking_type { get; set; }
        public string booking_name { get; set; }
        public string booking_email { get; set; }
        public Contact contact { get; set; }
        public List<string> notes { get; set; }
        public string product_language { get; set; }
        public string distributor_reference { get; set; }
        public string reservation_reference { get; set; }
    }

    public class RootObject
    {
        public string request_type { get; set; }
        public Data data { get; set; }
    }
}