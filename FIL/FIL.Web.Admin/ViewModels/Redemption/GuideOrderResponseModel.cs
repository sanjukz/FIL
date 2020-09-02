using System;
using System.Collections.Generic;
using FIL.Contracts.Models;

namespace FIL.Web.Kitms.Feel.ViewModels.Redemption
{
    public class GuideOrderResponseModel
    {
        public bool Success { get; set; }
        public List<FIL.Contracts.DataModels.Redemption.GuideOrderDetailsCustomModel> OrderDetails { get; set; }
        public List<FIL.Contracts.DataModels.User> ApprovedByUsers { get; set; }
    }
}
