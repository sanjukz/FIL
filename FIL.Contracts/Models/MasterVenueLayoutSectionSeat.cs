using FIL.Contracts.Enums;
using System;

namespace FIL.Contracts.Models
{
    public class MasterVenueLayoutSectionSeat
    {
        public long Id { get; set; }
        public Guid AltId { get; set; }
        public string SeatTag { get; set; }
        public int MasterVenueLayoutSectionId { get; set; }
        public string RowNumber { get; set; }
        public string ColumnNumber { get; set; }
        public int? RowOrder { get; set; }
        public int? ColumnOrder { get; set; }
        public SeatType SeatTypeId { get; set; }
    }
}