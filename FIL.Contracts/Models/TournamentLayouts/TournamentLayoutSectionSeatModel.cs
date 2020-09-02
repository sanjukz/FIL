using System.Collections.Generic;

namespace FIL.Contracts.Models.TournamentLayouts
{
    public class TournamentLayoutSectionSeatModel
    {
        public List<MasterVenueSeatItem> MasterVenueSeatItems { get; set; }
    }

    public class MasterVenueSeatItem
    {
        public long Id { get; set; }
        public string SeatTag { get; set; }
        public int TournamentLayoutSectionId { get; set; }
        public string RowNumber { get; set; }
        public string ColumnNumber { get; set; }
        public int? RowOrder { get; set; }
        public int? ColumnOrder { get; set; }
        public int SeatTypeId { get; set; }
    }
}