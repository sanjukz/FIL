using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.Boxoffice.TicketCategories;
using System;

namespace FIL.Contracts.Queries.BoxOffice.TicketCategory
{
    public class TicketCategoryQuery : IQuery<TicketCategoryQueryResult>
    {
        public Guid UserAltId { get; set; }
        public Guid EventDetailAltId { get; set; }
    }
}