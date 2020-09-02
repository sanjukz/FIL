using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.TicketCategories;
using System;

namespace FIL.Contracts.Queries.TicketCategory
{
    public class TicketCategoryQuery : IQuery<TicketCategoryQueryResult>
    {
        public Guid EventAltId { get; set; }
    }
}