using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.TournamentLayout;

namespace FIL.Contracts.Queries.TournamentLayout
{
    public class EventFeeTypeDetailsMappingQuery : IQuery<EventFeeTypeDetailsMappingQueryResult>
    {
        public long EventId { get; set; }
    }
}