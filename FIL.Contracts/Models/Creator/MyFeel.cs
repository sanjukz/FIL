using System;

namespace FIL.Contracts.Models.Creator
{
    public class MyFeel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public Guid AltId { get; set; }
        public FIL.Contracts.Enums.MasterEventType MasterEventType { get; set; }
        public int RoleId { get; set; }
        public string UserEmail { get; set; }
        public string Slug { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsTokenize { get; set; }
        public bool IsPastEvent { get; set; }
        public bool IsShowExclamationIcon { get; set; }
        public string CompletedStep { get; set; }
        public string CurrentStep { get; set; }
        public FIL.Contracts.Enums.EventStatus EventStatusId { get; set; }
        public DateTime EventStartDateTime { get; set; }
        public DateTime EventEndDateTime { get; set; }
        public DateTime EventCreatedDateTime { get; set; }
        public string EventStartDateTimeString { get; set; }
        public string EventEndDateTimeString { get; set; }
        public string TimeZoneAbbrivation { get; set; }
        public string TimeZoneOffset { get; set; }
        public string SubCategory { get; set; }
        public string EventUrl { get; set; }
        public string ParentCategory { get; set; }
        public int TicketForSale { get; set; }
        public int SoldTicket { get; set; }
    }
}