using System.Collections.Generic;

namespace FIL.Contracts.DataModels
{
    public class SeatLayoutData
    {
        public IEnumerable<SeatLayout> SeatLayouts { get; set; }
    }

    public class SeatLayout
    {
        public long Id { get; set; }
        public string SeatTag { get; set; }
        public string RowNumber { get; set; }
        public string ColumnNumber { get; set; }
        public int? RowOrder { get; set; }
        public int ColumnOrder { get; set; }
        public short SeatTypeId { get; set; }
        public bool IsEnabled { get; set; }
        public long EventTicketAttributeId { get; set; }
        public long EventTicketDetailId { get; set; }
        public int SeatStatusId { get; set; }
        public string SectionName { get; set; }
        public long? SponsorId { get; set; }
    }
}