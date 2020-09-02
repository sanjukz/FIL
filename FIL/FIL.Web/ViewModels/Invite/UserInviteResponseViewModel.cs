using FIL.Web.Feel.ViewModels.Default;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Feel.ViewModels.Invite
{
    public class UserInviteResponseViewModel : ResponseViewModel
    {
        public long Id { get; set; }
        public bool isUsed { get; set; }
    }
}
