using FIL.Contracts.Enums;
using System;

namespace FIL.Contracts.Models
{
    public class Transaction
    {
        public long Id { get; set; }
        public Guid AltId { get; set; }
        public string ChannelId { get; set; }
        public int CurrencyId { get; set; }
        public short? TotalTickets { get; set; }
        public decimal? GrossTicketAmount { get; set; }
        public decimal? DeliveryCharges { get; set; }
        public decimal? ConvenienceCharges { get; set; }
        public decimal? ServiceCharge { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? NetTicketAmount { get; set; }
        public decimal? DonationAmount { get; set; }
        public string DiscountCode { get; set; }
        public TransactionStatus? TransactionStatusId { get; set; }
        public string ReportExportStatus { get; set; }
        public int? IPDetailId { get; set; }
        public bool? IsEmailSend { get; set; }
        public bool? IsSmsSend { get; set; }
        public int OTP { get; set; }
        public bool? IsOTPVerified { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneCode { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailId { get; set; }
        public string CountryName { get; set; }
        public DateTime CreatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
    }
}