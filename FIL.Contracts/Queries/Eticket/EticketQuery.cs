using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.Eticket;

namespace FIL.Contracts.Queries.Eticket
{
    public class EticketQuery : IQuery<EticketQueryResult>
    {
        public long Id { get; set; }
        public string Email { get; set; }
    }
}