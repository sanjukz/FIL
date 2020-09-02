using FIL.Contracts.Enums;

namespace FIL.Contracts.Models
{
    public class TournamentLayoutSectionSeat
    {
        public long Id { get; set; }
        public string SeatTag { get; set; }
        public int TournamentLayoutSectionId { get; set; }
        public string RowNumber { get; set; }
        public string ColumnNumber { get; set; }
        public int? RowOrder { get; set; }
        public int? ColumnOrder { get; set; }
        public SeatType SeatTypeId { get; set; }
        public bool IsEnabled { get; set; }
    }
}