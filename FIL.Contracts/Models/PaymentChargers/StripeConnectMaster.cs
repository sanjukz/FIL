using System;

namespace FIL.Contracts.Models.PaymentChargers
{
    public class StripeConnectMaster
    {
        public long TransactionID { get; set; }
        public decimal GrossTicketAmount { get; set; }
        public decimal DeliveryCharges { get; set; }
        public decimal ConvenienceCharges { get; set; }
        public decimal ServiceCharge { get; set; }
        public long EventId { get; set; }
        public bool IsEnabled { get; set; }
        public string StripeConnectAccountID { get; set; }
        public int TotalTickets { get; set; }
        public decimal PricePerTicket { get; set; }
        public long TransactionDetailId { get; set; }
        public decimal ExtraCommisionFlat { get; set; }
        public decimal ExtraCommisionPercentage { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public decimal ChargebackHoldFlat { get; set; }
        public decimal ChargebackHoldPercentage { get; set; }
        public Guid CreatedBy { get; set; }
        public int PayoutDaysOffset { get; set; }
        public int ChargebackDaysOffset { get; set; }
    }
}