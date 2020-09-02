using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.CreateEventV1
{
    public class SponsorQueryResult
    {
        public bool Success { get; set; }
        public long EventId { get; set; }
        public bool IsValidLink { get; set; }
        public bool IsDraft { get; set; }
        public List<FIL.Contracts.DataModels.FILSponsorDetail> SponsorDetails { get; set; }
    }
}