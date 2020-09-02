using FIL.Contracts.Models;
using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.Account
{
    public class GetAllGuestUserDetailQueryResult
    {
        public List<GuestUserAdditionalDetail> GuestUserDetails { get; set; }
    }
}