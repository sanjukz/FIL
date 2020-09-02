using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.TournamentLayout;
using System;

namespace FIL.Contracts.Queries.TournamentLayout
{
    public class TournamentEventVenueQuery : IQuery<TournamentLayoutEventVenueQueryResult>
    {
        public Guid EventAltId { get; set; }
    }
}