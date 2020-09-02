using System;
using System.Collections.Generic;

namespace FIL.Contracts.DataModels
{
    public class TransactionsData
    {
        public IEnumerable<TransactionInfo> TransactionInfos { get; set; }
    }

    public class TransactionInfo
    {
        public string SrNo { get; set; }
        public string TransactionDetailId { get; set; }
        public Guid TransactionDetailAltId { get; set; }
        public string ConfirmationNumber { get; set; }
        public string CreatedUtc { get; set; }
        public string UserEmailId { get; set; }
        public string UserMobileNumber { get; set; }
        public string PhoneCode { get; set; }
        public string BuyerName { get; set; }
        public string EventName { get; set; }
        public string EventDate { get; set; }
        public string TicketCategoryName { get; set; }
        public string TotalTicket { get; set; }
        public string GrossTicketAmount { get; set; }
        public string PayConfNumber { get; set; }
        public string PaymentGateway { get; set; }
        public string Channel { get; set; }
        public string TransactionStatus { get; set; }
        public string CountryName { get; set; }
    }
}