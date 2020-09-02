using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.TournamentLayout;

namespace FIL.Contracts.Queries.TournamentLayout
{
    public class TournamentLayoutViewSeatLayoutQuery : IQuery<TournamentLayoutViewSeatLayoutQueryResult>
    {
        public int Id { get; set; }
    }
}