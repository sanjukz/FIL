using FIL.Contracts.Models;

namespace FIL.Contracts.QueryResults.Redemption
{
    public class BarcodeQueryResult
    {
        public BarcodeDetailsContainer BarcodeDetailsContainer { get; set; }
        public bool IsValid { get; set; }
    }
}