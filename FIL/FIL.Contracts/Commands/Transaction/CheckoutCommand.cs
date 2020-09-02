using FIL.Contracts.Enums;
using FIL.Contracts.Models.TMS;
using System;
using System.Collections.Generic;

namespace FIL.Contracts.Commands.Transaction
{
    public class CheckoutCommand : Contracts.Interfaces.Commands.ICommandWithResult<CheckoutCommandResult>
    {
        public Channels ChannelId { get; set; }
        public List<EventTicketAttribute> EventTicketAttributeList { get; set; }
        public GuestUserDetail GuestUser { get; set; }
        public Guid UserAltId { get; set; }
        public string Ip { get; set; }
        public Guid ModifiedBy { get; set; }
        public Site? SiteId { get; set; }
        public string InviteCode { get; set; }
        public bool ISRasv { get; set; }
        public int? OTPCode { get; set; }
        public bool IsLoginCheckout { get; set; }
        public bool? IsASI { get; set; }
        public bool IsQrTransaction { get; set; }
        public TransactionType TransactionType { get; set; }
        public string TransactionCurrency { get; set; }
        public string TargetCurrencyCode { get; set; }
        public List<SeatDetail> SeatDetails { get; set; }
        public decimal? DonationAmount { get; set; }
        public string ReferralId { get; set; }
        public bool IsBSPUpgrade { get; set; }
    }

    public class CheckoutCommandResult : Contracts.Interfaces.Commands.ICommandResult
    {
        public long Id { get; set; }
        public Guid TransactionAltId { get; set; }
        public bool Success { get; set; }
        public bool IsSeatSoldOut { get; set; }
        public bool IsBSPUpgraded { get; set; }
        public string EventName { get; set; }
        public string TicketCategoryName { get; set; }
        public bool IsTransactionLimitExceed { get; set; }
        public bool IsTicketCategorySoldOut { get; set; }
        public Models.User User { get; set; }
        public bool IsPaymentByPass { get; set; }
        public FIL.Contracts.Enums.StripeAccount StripeAccount { get; set; }
        public bool? IsASI { get; set; }
        public Models.ASI.ASIBookingResponse ASIBookingResponse { get; set; }
        public List<FIL.Contracts.Models.CartItemModel> CartBookingModel { get; set; }
        public FIL.Contracts.DataModels.Transaction Transaction { get; set; }
    }
}