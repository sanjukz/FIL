using FIL.Contracts.Models;
using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.BoxOffice
{
    public class GetUserRolesQueryResult
    {
        public List<Role> Roles { get; set; }
    }
}