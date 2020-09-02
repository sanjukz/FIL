using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.BoxOffice.TicketCategories;
using System;

namespace FIL.Contracts.Queries.BoxOffice.TicketCategory
{
    public class SeasonTicketCategoryQuery : IQuery<SeasonTicketCategoryQueryResult>
    {
        public Guid VenueAltId { get; set; }
        public Guid EventDetailAltId { get; set; }
        public int TicketCategoryId { get; set; }
    }
}