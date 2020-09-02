using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.BoxOffice;

namespace FIL.Contracts.Queries.BoxOffice
{
    public class GetTicketPickUpDetailsQuery : IQuery<GetTicketPickUpDetailsQueryResult>
    {
        public long TransactionId { get; set; }
    }
}