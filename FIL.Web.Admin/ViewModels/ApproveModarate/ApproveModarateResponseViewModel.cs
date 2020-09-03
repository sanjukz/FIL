using FIL.Contracts.Models;
using FIL.Contracts.Models.Zoom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Admin.ViewModels.ApproveModarate
{
    public class ApproveModarateResponseViewModel
    {
        public List<FIL.Contracts.Models.Event> Events { get; set; }
        public List<FIL.Contracts.Models.User> Users { get; set; }
        public List<LiveOnlineTransactionDetailResponseModel> MyFeelDetails { get; set; }
        public List<FIL.Contracts.DataModels.EventAttribute> EventAttributes { get; set; }
    }
}
