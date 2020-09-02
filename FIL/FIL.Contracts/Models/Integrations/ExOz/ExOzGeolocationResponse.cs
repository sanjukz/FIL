namespace FIL.Contracts.Models.Integrations.ExOz
{
    public class ExOzGeolocationResponse
    {
        public class Geolocation
        {
            public string Label { get; set; }
            public string Latitude { get; set; }
            public string Longitude { get; set; }
            public string Address { get; set; }
        }
    }
}