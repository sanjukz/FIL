using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Feel.ViewModels.Account
{
    public class UpdateNotificationViewModel
    {
        public Guid UserAltId { get; set; }
        public bool? ShouldUpdate { get; set; }
        public bool? IsOptedForMail { get; set; }
    }
}
