using System;
using System.Collections.Generic;

namespace FIL.Web.Feel.ViewModels.Payment
{
    public class PaymentOptionsResponseViewModel
    {
        public List<FIL.Contracts.Models.NetBankingBankDetail> BankDetails { get; set; }
        public List<FIL.Contracts.Models.CashCardDetail> CashCardDetails { get; set; }
    }
}
