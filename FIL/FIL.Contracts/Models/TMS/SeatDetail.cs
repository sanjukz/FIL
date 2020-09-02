namespace FIL.Contracts.Models.TMS
{
    public class SeatDetail
    {
        public long EventTicketDetailId { get; set; }
        public long MatchLayoutSectionSeatId { get; set; }
        public string SeatTag { get; set; }
        public short SeatTypeId { get; set; }
        public short TicketTypeId { get; set; }
    }
}