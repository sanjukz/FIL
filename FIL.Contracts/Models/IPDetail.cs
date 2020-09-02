namespace FIL.Contracts.Models
{
    public class IPDetail
    {
        public int Id { get; set; }
        public string IPAddress { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string RegionCode { get; set; }
        public string RegionName { get; set; }
        public string City { get; set; }
        public string Zipcode { get; set; }
        public string TimeZone { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string MetroCode { get; set; }
    }
}