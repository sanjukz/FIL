using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResult.TMS;
using System;
using System.Collections.Generic;
using System.Text;

namespace FIL.Contracts.Queries.Invoice
{
    public class InvoiceSponsorQuery : IQuery<SponsorQueryResult>
    {
        public Guid? EventDetailAltId { get; set; }
        public Guid? EventAltId { get; set; }
        public Guid? VenueAltId { get; set; }
     }
}
