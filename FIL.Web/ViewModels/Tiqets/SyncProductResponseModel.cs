using FIL.Contracts.Models.Tiqets;
using System.Collections.Generic;

namespace FIL.Web.Feel.ViewModels.Tiqets
{
    public class SyncProductResponseModel
    {
        public SyncProductResponse productResponse { get; set; }
        public bool success { get; set; }
        public int? remainingProducts { get; set; }
    }
    public class SyncProductResponse
    {
        public long Id { get; set; }
        public List<TiqetProduct> tiqetProducts { get; set; }
    }
}
