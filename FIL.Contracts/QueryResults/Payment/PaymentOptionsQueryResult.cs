using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.Payment
{
    public class PaymentOptionsQueryResult
    {
        public List<FIL.Contracts.Models.NetBankingBankDetail> BankDetails { get; set; }
        public List<FIL.Contracts.Models.CashCardDetail> CashCardDetails { get; set; }
    }
}