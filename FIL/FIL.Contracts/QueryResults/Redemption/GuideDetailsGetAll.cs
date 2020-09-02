using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.Redemption
{
    public class GuideDetailsGetAllResult
    {
        public List<FIL.Contracts.DataModels.Redemption.GuideDetailsCustom> GuideDetails { get; set; }
        public List<FIL.Contracts.DataModels.User> ApprovedByUsers { get; set; }
    }

    public class GuideDetailsCustomResult
    {
        public List<FIL.Contracts.DataModels.Redemption.GuideDetailsCustom> GuideDetails { get; set; }
        public List<FIL.Contracts.DataModels.User> ApprovedByUsers { get; set; }
    }
}