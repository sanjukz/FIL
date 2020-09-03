using FIL.Contracts.Models;
using FIL.Contracts.Models.Zoom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Admin.ViewModels.MyFeel
{
    public class MyFeelResponseViewModel
    {
        public List<FIL.Contracts.Models.Event> Events { get; set; }
        public List<FIL.Contracts.Models.EventDetail> EventDetails { get; set; }
        public List<LiveOnlineTransactionDetailResponseModel> MyFeelDetails { get; set; }
        public List<FIL.Contracts.DataModels.EventAttribute> EventAttributes { get; set; }
    }
}
