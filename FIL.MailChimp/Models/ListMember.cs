using MailChimp.Net.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FIL.MailChimp.Models
{
    public class ListMember: Member
    {

    }

    public enum MemberStatus
    {
        Undefined = 0,
        Subscribed = 1,
        Unsubscribed = 2,
        Cleaned = 3,
        Pending = 4,
        Transactional = 5,
        Archived = 6
    }
}
