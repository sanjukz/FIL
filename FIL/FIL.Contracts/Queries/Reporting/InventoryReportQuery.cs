using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.Reporting;
using System;

namespace FIL.Contracts.Queries.Reporting
{
    public class InventoryReportQuery : IQuery<InventoryReportQueryResult>
    {
        public Guid UserAltId { get; set; }
        public Guid EventAltId { get; set; }
        public Guid EventDetailAltId { get; set; }
    }
}