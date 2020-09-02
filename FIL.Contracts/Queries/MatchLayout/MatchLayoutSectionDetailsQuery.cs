﻿using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.MatchLayout;

namespace FIL.Contracts.Queries.MatchLayout
{
    public class MatchLayoutSectionDetailsQuery : IQuery<MatchLayoutSectionDetailsQueryResult>
    {
        public int EventId { get; set; }
        public int VenueId { get; set; }
        public int EventDetailId { get; set; }
    }
}