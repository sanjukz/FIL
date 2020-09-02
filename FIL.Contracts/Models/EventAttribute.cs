namespace FIL.Contracts.Models
{
    public class EventAttribute
    {
        public long Id { get; set; }
        public long EventDetailId { get; set; }
        public short? MatchNo { get; set; }
        public short? MatchDay { get; set; }
        public string GateOpenTime { get; set; }
        public string TimeZone { get; set; }
        public string TimeZoneAbbreviation { get; set; }
        public string TicketHtml { get; set; }
        public string MatchAdditionalInfo { get; set; }
    }
}