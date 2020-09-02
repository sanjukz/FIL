using FIL.Contracts.Enums;
using System;
using System.Collections.Generic;

namespace FIL.Contracts.Models.Creator
{
    public class FILTransactionLocator
    {
        public List<TransactionData> TransactionData { get; set; }
    }

    public class TransactionData
    {
        public long TransactionId { get; set; }
        public Guid TransactionAltId { get; set; }
        public DateTime TransactionCreatedUtc { get; set; }
        public string Email { get; set; }
        public string PhoneCode { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EventName { get; set; }
        public DateTime EventStartDate { get; set; }
        public DateTime EventEndDate { get; set; }
        public string LocalStartDateString { get; set; }
        public string LocalEndDateString { get; set; }
        public EventFrequencyType EventFrequencyType { get; set; }
        public bool IsEventEnabled { get; set; }
        public bool IsEventDetailEnabled { get; set; }
        public long eventId { get; set; }
        public Guid EventAltId { get; set; }
        public MasterEventType MasterEventTypeId { get; set; }
        public EventStatus EventStatusId { get; set; }
        public string TicketCategoryName { get; set; }
        public int TotalTicket { get; set; }
        public string TimeZone { get; set; }
        public string TimeZoneAbbreviation { get; set; }
        public string CurrencyCode { get; set; }
        public decimal? GrossTicketAmount { get; set; }
        public decimal? NetTicketAmount { get; set; }
        public decimal? DiscountAmount { get; set; }
        public string PromoCode { get; set; }
        public string PayConfNumber { get; set; }
        public string Channel { get; set; }
        public string TransactionStatus { get; set; }
        public Guid? StreamAltId { get; set; }
        public string IPAddress { get; set; }
        public string IpCity { get; set; }
        public string IpState { get; set; }
        public string IpCountry { get; set; }
    }

}