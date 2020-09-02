using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults;

namespace FIL.Contracts.Queries.Stand
{
    public class StandQuery : IQuery<StandQueryResult>
    {
        public int MasterVenueLayoutId { get; set; }
    }
}