using FIL.Contracts.Enums;
using System;

namespace FIL.Contracts.Models.Report
{
    public class TicketCategoryContainer
    {
        public string EventName { get; set; }
        public string EventSource { get; set; }
        public DateTime EventDateTime { get; set; }
        public string TicketCategoryName { get; set; }
        public TicketType TicketType { get; set; }
        public int TicketQuantity { get; set; }
        public decimal TicketPrice { get; set; }
        public string CurrencyCode { get; set; }
        public decimal? ExchangeRate { get; set; }
        public string SeatNumbers { get; set; }
    }
}