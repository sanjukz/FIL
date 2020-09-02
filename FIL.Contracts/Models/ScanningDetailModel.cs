using System;

namespace FIL.Contracts.Models
{
    public class ScanningDetailModel
    {
        public long Id { get; set; }
        public long MatchLayoutSectionSeatId { get; set; }
        public long EventTicketDetailId { get; set; }
        public decimal Price { get; set; }
        public string ChannelId { get; set; }
        public int SeatStatusId { get; set; }
        public string BarcodeNumber { get; set; }
        public int? EntryCount { get; set; }
        public bool EntryStatus { get; set; }
        public DateTime? EntryDateTime { get; set; }
        public string EntryGateName { get; set; }
        public string TicketTypeId { get; set; }
        public long TransactionId { get; set; }
        public long SponsorId { get; set; }
        public int? OutCount { get; set; }
        public DateTime? OutDateTime { get; set; }
        public decimal? Value { get; set; }
        public bool? IsConsumed { get; set; }
        public DateTime? ConsumedDateTime { get; set; }
        public int? EntryCount2 { get; set; }
    }
}