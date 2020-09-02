using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.TournamentLayout;
using System;

namespace FIL.Contracts.Queries.TournamentLayout
{
    public class SaveTournamentLayoutSectionQuery : IQuery<SaveTournamentLayoutSectionQueryResult>
    {
        public int EventId { get; set; }
        public int VenueId { get; set; }
        public string SectionIds { get; set; }

        //public string SectionData { get; set; }
        public Guid CreatedBy { get; set; }
    }
}