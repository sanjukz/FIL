namespace FIL.Contracts.Models.CitySightSeeing
{
    public class CitySightSeeingRouteDetail
    {
        public long Id { get; set; }
        public long CitySightSeeingRouteId { get; set; }
        public string RouteLocationName { get; set; }
        public string RouteLocationId { get; set; }
        public string RouteLocationDescription { get; set; }
        public string RouteLocationLatitude { get; set; }
        public string RouteLocationLongitude { get; set; }
        public bool RouteLocationStopover { get; set; }
        public bool IsEnabled { get; set; }
    }
}