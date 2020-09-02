using System;
using System.Collections.Generic;

namespace FIL.Contracts.Commands.Transaction
{
    public class TransactionReportCommand : Contracts.Interfaces.Commands.ICommandWithResult<TransactionReportCommandResult>
    {
        public Guid UserAltId { get; set; }
        public Guid EventAltId { get; set; }
        public long EventDetailId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int PageNumber { get; set; }
        public int NoRecordsPerPage { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class TransactionReportCommandResult : Contracts.Interfaces.Commands.ICommandResult
    {
        public long Id { get; set; }
        public IEnumerable<FIL.Contracts.Commands.Transaction.TransactionReport> TransactionReport { get; set; }
    }

    public class TransactionReport
    {
        public long EventTransId { get; set; }
        public string EmailId { get; set; }
        public string IPAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string CustomerName { get; set; }
        public string ModeOfPayment { get; set; }
        public string CardType { get; set; }
        public string SaleStatus { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedTime { get; set; }
        public string EventName { get; set; }
        public string SubEventName { get; set; }
        public string EventCity { get; set; }
        public string Channels { get; set; }
        public string CurrencyName { get; set; }
        public string OutletName { get; set; }
        public string CustomerCountry { get; set; }
        public string CustomerState { get; set; }
        public string CustomerCity { get; set; }
        public string CardIssuingCountry { get; set; }
        public string SuspectTransaction { get; set; }
        public string EventDate { get; set; }
        public string VenueAddress { get; set; }
        public string Promocode { get; set; }
        public string TicketCategoty { get; set; }
        public string SeatNumber { get; set; }
        public string TicketType { get; set; }
        public short NumberOfTickets { get; set; }
        public decimal PricePerTicket { get; set; }
        public decimal GrossTicketAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal NetTicketAmount { get; set; }
        public decimal ConvenienceCharges { get; set; }
        public decimal ServiceTax { get; set; }
        public decimal TotalTransactedAmount { get; set; }
        public decimal CourierCharge { get; set; }
        public string DeliveryType { get; set; }
        public string TransactionType { get; set; }
        public string PaymentGateway { get; set; }
        public string PayConfNumber { get; set; }
        public short EntryCount { get; set; }
        public string NameOnCard { get; set; }
    }
}