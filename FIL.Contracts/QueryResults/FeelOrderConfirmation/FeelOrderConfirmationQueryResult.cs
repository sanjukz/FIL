using FIL.Contracts.Models;
using FIL.Contracts.Models.Zoom;
using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.FeelOrderConfirmation
{
    public class FeelOrderConfirmationQueryResult
    {
        public List<ZoomHostUserModel> hostUsersModel { get; set; }
        public LiveOnlineTransactionDetailResponseModel liveOnlineDetailModel { get; set; }
        public FIL.Contracts.Models.Transaction Transaction { get; set; }
        public TransactionPaymentDetail TransactionPaymentDetail { get; set; }
        public UserCardDetail UserCardDetail { get; set; }
        public CurrencyType CurrencyType { get; set; }
        public string PaymentOption { get; set; }
        public string cardTypes { get; set; }
        public List<OrderConfirmationSubContainer> orderConfirmationSubContainer { get; set; }
        public List<EventAteendeeDetail> eventAttendeeDetail { get; set; }
        public bool IsTiqets { get; set; }
        public bool IsHoho { get; set; }
        public bool IsLiveOnline { get; set; }
        public FIL.Contracts.DataModels.ZoomUser ZoomUser { get; set; }
    }
}