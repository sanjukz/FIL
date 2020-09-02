using System.Collections.Generic;

namespace FIL.Contracts.Models.CitySightSeeing
{
    public class CompanyOpeningTime
    {
        public string day { get; set; }
        public string start_from { get; set; }
        public string end_to { get; set; }
    }

    public class SubOption
    {
        public string option_id { get; set; }
        public string option_name { get; set; }
        public string option_price { get; set; }
    }

    public class ExtraOption
    {
        public string extra_option_id { get; set; }
        public string extra_option_name { get; set; }
        public string extra_option_type { get; set; }
        public int is_mandatory { get; set; }
        public List<SubOption> options { get; set; }
    }

    public class TicketTypeDetail
    {
        public string ticket_type { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }
        public int age_from { get; set; }
        public int age_to { get; set; }
        public string unit_price { get; set; }
        public string unit_list_price { get; set; }
        public string unit_discount { get; set; }
        public string unit_gross_price { get; set; }
        public List<ExtraOption> extra_options { get; set; }
    }

    public class Route
    {
        public string route_id { get; set; }
        public string route_name { get; set; }
        public string route_color { get; set; }
        public string route_duration { get; set; }
        public string route_type { get; set; }
        public string route_start_time { get; set; }
        public string route_end_time { get; set; }
        public string route_frequency { get; set; }
        public List<string> route_audio_languages { get; set; }
        public List<string> route_live_languages { get; set; }
        public List<RouteLocation> route_locations { get; set; }
    }

    public class RouteLocation
    {
        public string route_location_id { get; set; }
        public string route_location_name { get; set; }
        public string route_location_description { get; set; }
        public string route_location_latitude { get; set; }
        public string route_location_longitude { get; set; }
        public bool route_location_stopover { get; set; }
    }

    public class TicketDetailData
    {
        public string ticket_id { get; set; }
        public string ticket_title { get; set; }
        public string short_description { get; set; }
        public string long_description { get; set; }
        public List<string> highlights { get; set; }
        public string duration { get; set; }
        public string product_language { get; set; }
        public string txt_language { get; set; }
        public string ticket_entry_notes { get; set; }
        public List<object> tags { get; set; }
        public List<object> included { get; set; }
        public List<CompanyOpeningTime> company_opening_times { get; set; }
        public string book_size_min { get; set; }
        public string book_size_max { get; set; }
        public string supplier_url { get; set; }
        public int ticket_class { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }
        public string booking_start_date { get; set; }
        public List<string> images { get; set; }
        public string currency { get; set; }
        public string pickup_points { get; set; }
        public string combi_ticket { get; set; }
        public List<TicketTypeDetail> ticket_type_details { get; set; }
        public List<Route> routes { get; set; }
    }

    public class TicketDetails
    {
        public string response_type { get; set; }
        public TicketDetailData data { get; set; }
    }
}