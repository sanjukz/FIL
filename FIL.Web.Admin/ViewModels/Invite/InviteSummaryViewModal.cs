using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Admin.ViewModels.Invite
{
    public class InviteSummaryViewModal
    {
        public long TotalMails { get; set; }

        public long UsedMails { get; set; }

        public long UnUsedMails { get; set; }
    }
}
