using FIL.Contracts.Interfaces.Commands;
using System;
using System.Collections.Generic;

namespace FIL.Contracts.Commands.Feel
{
    public class SaveFeelBarcodeCommand : ICommandWithResult<SaveFeelBarcodeCommandResult>
    {
        public long TransactionId { get; set; }
        public Guid? TransactionAltId { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class SaveFeelBarcodeCommandResult : ICommandResult
    {
        public bool Success { get; set; }
        public long Id { get; set; }
        public List<PAHDetail> PahDetail { get; set; }
    }

    public class PAHDetail
    {
        public long TransactionId { get; set; }
        public long TotalTickets { get; set; }
        public long VenueId { get; set; }
        public string VenueName { get; set; }
        public string CityName { get; set; }
        public string StateName { get; set; }
        public string CountryName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailId { get; set; }
        public string PhoneNumber { get; set; }
        public long EventId { get; set; }
        public string EventName { get; set; }
        public long EventDeatilId { get; set; }
        public string EventDetailsName { get; set; }
        public string TimeSlot { get; set; }
        public DateTime EventStartTime { get; set; }
        public long EventsourceId { get; set; }
        public string TicketHtml { get; set; }
        public long TicketCategoryId { get; set; }
        public string TicketCategoryName { get; set; }
        public decimal Price { get; set; }
        public string BarcodeNumber { get; set; }
        public string CurrencyName { get; set; }
        public List<CategoryWiseTickets> CategoryWiseTickets { get; set; }
    }

    public class CategoryWiseTickets
    {
        public string CategoryName { get; set; }
        public int TotalTickets { get; set; }
    }
}