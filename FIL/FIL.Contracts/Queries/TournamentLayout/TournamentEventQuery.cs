using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.TournamentLayout;
using System;

namespace FIL.Contracts.Queries.TournamentLayout
{
    public class TournamentEventQuery : IQuery<TournamentLayoutEventQueryResult>
    {
        public Guid AltId { get; set; }
        public bool IsMyFeel { get; set; }
    }
}