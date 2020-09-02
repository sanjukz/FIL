using System.Collections.Generic;
using FIL.Contracts.Enums;

namespace FIL.Contracts.Models
{
    public class Row
    {
        public List<SeatItem> SeatItems { get; set; }
    }

    public class SeatItem
    {
        public long Id { get; set; }
        public string SeatTag { get; set; }
        public string RowNumber { get; set; }
        public string ColumnNumber { get; set; }
        public int? RowOrder { get; set; }
        public int? ColumnOrder { get; set; }
        public SeatType SeatTypeId { get; set; }
        public int SeatStatusId { get; set; }
        public long EventTicketDetailId { get; set; }
        public long? SponsorId { get; set; }
    }
}
