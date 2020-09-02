using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults;
using System;

namespace FIL.Contracts.Queries.MatchLayout
{
    public class SaveMatchLayoutSectionQuery : IQuery<SaveMatchLayoutSectionQueryResult>
    {
        public int EventId { get; set; }
        public int VenueId { get; set; }
        public string EventDetailId { get; set; }
        public string sectionData { get; set; }
        public Guid CreatedBy { get; set; }
        public string feeDetails { get; set; }
    }
}