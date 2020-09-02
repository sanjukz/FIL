using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.TicketPrint;

namespace FIL.Contracts.Queries.TicketPrint
{
    public class TicketPrintQuery : IQuery<TicketPrintQueryResult>
    {
        public long TranscationId { get; set; }
        public Channels channelId { get; set; }
    }
}