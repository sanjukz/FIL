using FIL.Contracts.Models;
using System;
using System.Collections.Generic;

namespace FIL.Web.Feel.ViewModels.Transaction
{
    public class TransactionResponseViewModel
    {
        public long Id { get; set; }
        public Guid AltId { get; set; }
        public List<long> Transactions { get; set; }
        public string ChannelId { get; set; }
        public string Currency { get; set; }
        public short? TotalTickets { get; set; }
        public decimal? GrossTicketAmount { get; set; }
        public decimal? DeliveryCharges { get; set; }
        public decimal? ConvenienceCharges { get; set; }
        public decimal? ServiceCharge { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? NetTicketAmount { get; set; }
        public decimal? DonationAmount { get; set; }
        public string TransactionStatusId { get; set; }
        public List<Event> Events { get; set; }
    }
}
