using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults;
using System;

namespace FIL.Contracts.Queries.TicketAlert
{
    public class TicketAlertQuery : IQuery<TicketAlertQueryResult>
    {
        public Guid altId { get; set; }
    }
}