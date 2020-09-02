using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.Invoice;
using System;

namespace FIL.Contracts.Queries.Invoice
{
    public class SponsorOrderDetailsQuery : IQuery<SponsorOrderDetailsQueryResult>
    {
        public Guid eventAltId { get; set; }
        public Guid venueAltId { get; set; }
        public long sponsorId { get; set; }
    }
}