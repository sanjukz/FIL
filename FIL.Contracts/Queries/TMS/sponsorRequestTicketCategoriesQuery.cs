using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.TMS;
using System;

namespace FIL.Contracts.Queries.TMS
{
    public class SponsorRequestTicketCategoriesQuery : IQuery<sponsorRequestTicketCategoriesQueryResult>
    {
        public Guid EventDetailAltId { get; set; }
    }
}