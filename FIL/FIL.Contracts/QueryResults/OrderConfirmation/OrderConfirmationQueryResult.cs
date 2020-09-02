using FIL.Contracts.Models;
using FIL.Contracts.Models.Zoom;
using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.OrderConfirmation
{
    public class OrderConfirmationQueryResult
    {
        public FIL.Contracts.Models.Transaction Transaction { get; set; }
        public TransactionPaymentDetail TransactionPaymentDetail { get; set; }
        public UserCardDetail UserCardDetail { get; set; }
        public CurrencyType CurrencyType { get; set; }
        public string PaymentOption { get; set; }
        public string cardTypes { get; set; }
        public List<OrderConfirmationSubContainer> orderConfirmationSubContainer { get; set; }
        public int TicketQuantity { get; set; }
        public decimal GoodsAndServiceTax { get; set; }
        public bool IsASI { get; set; }
        public bool IsHoho { get; set; }
        public bool IsTiqets { get; set; }
        public FIL.Contracts.DataModels.ZoomUser ZoomUser { get; set; }
        public List<ZoomHostUserModel> hostUsersModel { get; set; }
        public LiveOnlineTransactionDetailResponseModel liveOnlineDetailModel { get; set; }
    }
}