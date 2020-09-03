using FIL.Contracts.Models;
using FIL.Web.Admin.ViewModels.Default;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Admin.ViewModels.Invite
{
    public class InviteResponseViewModel : ResponseViewModel
    {
        public List<UsersWebsiteInvite> Invites { get; set; }
    }

    
}
