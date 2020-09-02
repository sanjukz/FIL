using System.Collections.Generic;

namespace FIL.Contracts.DataModels
{
    public class SeasonSeatInfo
    {
        public IEnumerable<SeasonSeat> SeasonSeats { get; set; }
    }

    public class SeasonSeat
    {
        public long EventTicketDetailId { get; set; }
        public long MatchLayoutSectionSeatId { get; set; }
        public string SeatTag { get; set; }
        public long CompanionMatchLayoutSectionSeatId { get; set; }
        public string CompanionSeatTag { get; set; }
        public string SeatTypeId { get; set; }
    }
}