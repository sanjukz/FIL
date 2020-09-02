using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.MatchLayout;
using System;

namespace FIL.Contracts.Queries.MatchLayout
{
    public class MatchLayoutGetTournamentQuery : IQuery<MatchLayoutGetTournamentQueryResult>
    {
        public Guid AltId { get; set; }
    }
}