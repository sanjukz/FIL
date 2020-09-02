using System;
using System.Collections.Generic;

namespace FIL.Contracts.DataModels
{
    public class MatchSeatTicketData
    {
        public IEnumerable<MatchSeatTicketInfo> matchSeatTicketInfo { get; set; }
        public IEnumerable<MatchTeamInfo> matchTeamInfo { get; set; }
    }

    public class MatchSeatTicketInfo
    {
        public long VenueId { get; set; }
        public string VenueName { get; set; }
        public string CityName { get; set; }
        public long EventId { get; set; }
        public string EventName { get; set; }
        public long EventDeatilId { get; set; }
        public string EventDetailsName { get; set; }
        public DateTime EventStartTime { get; set; }
        public string TicketHtml { get; set; }
        public string GateOpenTime { get; set; }
        public string MatchNo { get; set; }
        public string MatchDay { get; set; }
        public string MatchAdditionalInfo { get; set; }
        public string SponsorOrCustomerName { get; set; }
        public int TicketCategoryId { get; set; }
        public string TicketCategoryName { get; set; }
        public decimal Price { get; set; }
        public string BarcodeNumber { get; set; }
        public string CurrencyName { get; set; }
        public string StandName { get; set; }
        public string LevelName { get; set; }
        public string BlockName { get; set; }
        public string SectionName { get; set; }
        public string GateName { get; set; }
        public string RowNumber { get; set; }
        public string TicketNumber { get; set; }
        public short TicketTypeId { get; set; }
        public long IsCompanion { get; set; }
        public long IsWheelChair { get; set; }
        public bool IsSeatSelection { get; set; }
        public int TransactingOptionId { get; set; }
        public long TransactionId { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string BoAddress { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TimeZone { get; set; }
    }

    public class MatchTeamInfo
    {
        public string TeamA { get; set; }
        public string TeamB { get; set; }
        public string MatchNo { get; set; }
        public DateTime MatchStartTime { get; set; }
        public long EventDetailId { get; set; }
    }
}