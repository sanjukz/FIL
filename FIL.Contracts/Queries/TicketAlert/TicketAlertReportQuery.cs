using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.TicketAlert;
using System;

namespace FIL.Contracts.Queries.TicketAlert
{
    public class TicketAlertReportQuery : IQuery<TicketAlertReportQueryResult>
    {
        public Guid AltId { get; set; }
    }
}