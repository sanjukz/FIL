using System.Collections.Generic;

namespace FIL.Contracts.DataModels
{
    public class TransactionReport
    {
        public IEnumerable<ReportingColumn> ReportColumns { get; set; }
        public IEnumerable<ReportingColumn> SummaryColumns { get; set; }
        public IEnumerable<ReportingColumn> DynamicSummaryColumns { get; set; }
        public IEnumerable<ReportingColumn> DynamicSummaryInfoColumns { get; set; }
        public IEnumerable<TransactionData> TransactionData { get; set; }
        public IEnumerable<TransactionData> CurrencyWiseSummary { get; set; }
        public IEnumerable<TransactionData> ChannelWiseSummary { get; set; }
        public IEnumerable<TransactionData> TicketTypeWiseSummary { get; set; }
        public IEnumerable<TransactionData> VenueWiseSummary { get; set; }
        public IEnumerable<TransactionData> EventWiseSummary { get; set; }
    }

    public class TransactionData
    {
        public long Sno { get; set; }
        public long TransactionId { get; set; }
        public string TransactionDate { get; set; }
        public string TransactionTime { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public string OutletBOName { get; set; }
        public string Channel { get; set; }
        public string Event { get; set; }
        public string EventName { get; set; }
        public string EventStartDateTime { get; set; }
        public string VenueAddress { get; set; }
        public string VenueCity { get; set; }
        public string VenueCountry { get; set; }
        public string TicketCategory { get; set; }
        public string SeatNumber { get; set; }
        public string TicketType { get; set; }
        public string DeliveryType { get; set; }
        public string Currency { get; set; }
        public decimal PricePerTicket { get; set; }
        public int NumberOfTickets { get; set; }
        public decimal GrossTicketAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal DonationAmount { get; set; }
        public string PromoCode { get; set; }
        public decimal NetTicketAmount { get; set; }
        public decimal NetTicketAmountUSD { get; set; }
        public decimal ServiceCharge { get; set; }
        public decimal DeliveryCharges { get; set; }
        public decimal ConvenienceCharges { get; set; }
        public decimal ExchangeRate { get; set; }
        public decimal TotalTransactedAmount { get; set; }
        public decimal TotalTransactedAmountUSD { get; set; }
        public string CustomerIP { get; set; }
        public string IPBasedCountry { get; set; }
        public string IPBasedState { get; set; }
        public string IPBasedCity { get; set; }
        public string SaleStatus { get; set; }
        public string PayConfNumber { get; set; }
        public string TransactionType { get; set; }
        public string CardNumber { get; set; }
        public string NameOnCard { get; set; }
        public string PaymentGateway { get; set; }
        public string CardType { get; set; }
        public string ModeOfPayment { get; set; }
        public string MembershipId { get; set; }
        public string PickUpAddress { get; set; }
        public string ReferralName { get; set; }
        public string StreamLink { get; set; }
    }
}