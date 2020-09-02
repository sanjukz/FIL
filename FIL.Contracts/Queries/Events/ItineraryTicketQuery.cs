using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.ItineraryTicket;
using System;
using System.Collections.Generic;

namespace FIL.Contracts.Queries.ItineraryTicket
{
    public class ItineraryTicketQuery : IQuery<ItineraryTicketQueryResult>
    {
        public List<Int64> eventIds { get; set; }
    }
}