using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Admin.ViewModels.Invite
{
    public class UpdateInviteRequestViewModel
    {
        public string Email { get; set; }
        public string InviteCode { get; set; }
        public bool IsUsed { get; set; }
        public int Id { get; set; }
    }
}
