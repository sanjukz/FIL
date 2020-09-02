using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.Redemption
{
    public class GuideOrderDetailsQueryResult
    {
        public List<FIL.Contracts.DataModels.Redemption.GuideOrderDetailsCustomModel> OrderDetails { get; set; }
        public List<FIL.Contracts.DataModels.User> ApprovedByUsers { get; set; }
    }
}