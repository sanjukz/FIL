using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.UserOrder;
using System;

namespace FIL.Contracts.Queries.UserOrders
{
    public class UserOrdersQuery : IQuery<UserOrderQueryResult>
    {
        public Guid UserAltId { get; set; }
    }
}