namespace FIL.Contracts.Commands.EventWizard
{
    public class EventAttributeCommand : BaseCommand
    {
        public short? MatchNo { get; set; }
        public short? MatchDay { get; set; }
        public string GateOpenTime { get; set; }
        public string TimeZone { get; set; }
        public string TimeZoneAbbreviation { get; set; }
        public string TicketHtml { get; set; }
    }
}