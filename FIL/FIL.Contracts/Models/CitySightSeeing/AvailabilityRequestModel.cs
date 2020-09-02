namespace FIL.Contracts.Models.CitySightSeeing
{
    public class Data
    {
        public string distributor_id { get; set; }
        public string ticket_id { get; set; }
        public string from_date { get; set; }
        public string to_date { get; set; }
    }

    public class AvailabilityRequestModel
    {
        public string request_type { get; set; }
        public Data data { get; set; }
    }
}