using System;

namespace FIL.Contracts.Models
{
    public class MatchSeatTicketDetail
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
    }
}