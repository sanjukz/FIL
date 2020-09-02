using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.SponsersSeat;

namespace FIL.Contracts.Queries.SponsersSeat
{
    public class SponsersSeatQuery : IQuery<SponsersSeatQueryResult>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}