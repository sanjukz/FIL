using FIL.Contracts.Interfaces.Commands;
using System;
using System.Collections.Generic;

namespace FIL.Contracts.Commands.BoxOffice
{
    public class PrintTicketCommand : ICommandWithResult<PrintBarcodeCommandResult>
    {
        public long TransactionId { get; set; }
        public Guid? TransactionAltId { get; set; }
        public long MatchSeatTicketDetailId { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class PrintBarcodeCommandResult : ICommandResult
    {
        public bool Success { get; set; }
        public long Id { get; set; }
        public List<TicketDetail> TicketDetail { get; set; }
    }

    public class TicketDetail
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
        public long TicketCategoryId { get; set; }
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
        public string TicketType { get; set; }
        public long IsWheelChair { get; set; }
        public bool? IsSeatSelection { get; set; }
        public string match1teamA { get; set; }
        public string match1teamB { get; set; }
        public string match2teamA { get; set; }
        public string match2teamB { get; set; }
        public string match3teamA { get; set; }
        public string match3teamB { get; set; }
        public string match1Time { get; set; }
        public string match2Time { get; set; }
        public string match3Time { get; set; }
    }
}