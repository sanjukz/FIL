using FIL.Contracts.Models;
using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.TicketLookup
{
    public class TicketLookupPhoneDetailQueryResult
    {
        public List<TicketLookupEmailDetailContainer> TicketLookupEmailDetailContainer { get; set; }
    }
}