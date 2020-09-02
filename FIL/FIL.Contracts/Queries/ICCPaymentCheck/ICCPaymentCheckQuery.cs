using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.ICCPaymentCheck;
using System.Collections.Generic;

namespace FIL.Contracts.Queries.ICCPaymentCheck
{
    public class ICCPaymentCheckQuery : IQuery<ICCPaymentCheckQueryResult>
    {
        public string UserEmail { get; set; }
        public string UserPhone { get; set; }
        public string TicketLimitError { get; set; }
        public List<EventTicketAttribute> eventTicketAttribute { get; set; }
    }

    public class EventTicketAttribute
    {
        public long Id { get; set; }
        public short TotalTickets { get; set; }
    }
}