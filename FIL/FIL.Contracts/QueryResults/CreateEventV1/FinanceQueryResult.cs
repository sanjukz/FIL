using System;

namespace FIL.Contracts.QueryResults.CreateEventV1
{
    public class FinanceQueryResult
    {
        public long EventId { get; set; }
        public Guid EventAltId { get; set; }
        public string IsoAlphaTwoCode { get; set; }
        public bool Success { get; set; }
        public bool IsValidLink { get; set; }
        public bool IsDraft { get; set; }
        public FIL.Contracts.Enums.StripeAccount StripeAccount { get; set; }
        public string StripeConnectAccountId { get; set; }
        public FIL.Contracts.Models.CreateEventV1.EventFinanceDetailModel EventFinanceDetailModel { get; set; }
        public FIL.Contracts.DataModels.CurrencyType CurrencyType { get; set; }
    }
}