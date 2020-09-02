using System.Collections.Generic;

namespace FIL.Contracts.Models.MatchLevel
{
    public class MatchLayoutSeatModel
    {
        public List<MatchLevelSeatItem> MasterVenueSeatItems { get; set; }
    }

    public class MatchLevelSeatItem
    {
        public long Id { get; set; }
        public string SeatTag { get; set; }
        public int MatchLayoutSectionId { get; set; }
        public string RowNumber { get; set; }
        public string ColumnNumber { get; set; }
        public int? RowOrder { get; set; }
        public int? ColumnOrder { get; set; }
        public int SeatTypeId { get; set; }
        public int? EventTicketDetailId { get; set; }
    }
}