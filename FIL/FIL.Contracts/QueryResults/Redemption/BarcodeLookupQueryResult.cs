using FIL.Contracts.Models;
using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.Redemption
{
    public class BarcodeLookupQueryResult
    {
        public List<BarcodeDetailsContainer> BarcodeDetailsContainer { get; set; }
        public bool IsValid { get; set; }
    }
}