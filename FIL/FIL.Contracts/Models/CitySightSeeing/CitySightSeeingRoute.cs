namespace FIL.Contracts.Models.CitySightSeeing
{
    public class CitySightSeeingRoute
    {
        public long Id { get; set; }
        public long EventDetailId { get; set; }
        public string RouteId { get; set; }
        public string RouteName { get; set; }
        public string RouteColor { get; set; }
        public string RouteDuration { get; set; }
        public string RouteType { get; set; }
        public string RouteStartTime { get; set; }
        public string RouteEndTime { get; set; }
        public string RouteFrequency { get; set; }
        public string RouteLiveLanguages { get; set; }
        public string RouteAudioLanguages { get; set; }
        public bool IsEnabled { get; set; }
    }
}