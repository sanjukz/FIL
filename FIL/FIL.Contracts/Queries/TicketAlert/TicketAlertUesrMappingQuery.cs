using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.TicketAlert;
using System;
using System.Collections.Generic;

namespace FIL.Contracts.Queries.TicketAlert
{
    public class TicketAlertUesrMappingQuery : IQuery<TicketAlertUesrMappingQueryResult>
    {
        public Guid EventAltId { get; set; }
        public string Email { get; set; }
        public long EventDetailId { get; set; }
        public List<string> Countries { get; set; }
        public List<int> TicketAlertEvents { get; set; }
    }
}