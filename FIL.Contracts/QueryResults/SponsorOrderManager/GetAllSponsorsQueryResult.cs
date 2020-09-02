using System.Collections.Generic;

namespace FIL.Contracts.QueryResult.SponsorOrderManager
{
    public class GetAllSponsorsQueryResult
    {
        public List<FIL.Contracts.Models.Sponsor> sponsors { get; set; }
    }
}