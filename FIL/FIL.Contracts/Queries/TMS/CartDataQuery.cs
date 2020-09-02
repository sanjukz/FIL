using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResult.TMS;
using System.Collections.Generic;

namespace FIL.Contracts.Queries.TMS
{
    public class CartDataQuery : IQuery<CartDataQueryResult>
    {
        public List<long> EventDetailids { get; set; }
        public int TicketCategoryId { get; set; }
    }
}