namespace FIL.Contracts.Models.Zoom
{
    public class ZoomCreateMeetingModel
    {
        public string topic { get; set; }
        public string start_time { get; set; }
        public int duration { get; set; }
        public string timezone { get; set; }
        public string password { get; set; }
        public string agenda { get; set; }
        public Settings settings { get; set; }
    }
}