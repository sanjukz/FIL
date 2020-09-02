using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.BoxOffice;

namespace FIL.Contracts.Queries.BoxOffice
{
    public class GetTicketDataQuery : IQuery<GetTicketDataQueryResult>
    {
        public string BarcodeNumber { get; set; }
        public bool IsRefund { get; set; }
    }
}