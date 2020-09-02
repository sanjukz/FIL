using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.Invoice
{
    public class BankDetailsQueryResult
    {
        public List<BankDetail> BankDetails { get; set; }
    }
}