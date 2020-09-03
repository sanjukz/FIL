using System;
using System.Collections.Generic;
using FIL.Contracts.Models;
using FIL.Contracts.QueryResults.Redemption;

namespace FIL.Web.Admin.ViewModels.Redemption
{
    public class GuideDetailsGetAllResponseViewModal
    {
        public List<FIL.Contracts.DataModels.Redemption.GuideDetailsCustom> GuideDetails { get; set; }
        public List<FIL.Contracts.DataModels.User> ApprovedByUsers { get; set; }
    }
}
