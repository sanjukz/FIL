using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.DeliveryOptions;
using System;

namespace FIL.Contracts.Queries.DeliveryOptions
{
    public class DeliveryOptionsQuery : IQuery<DeliveryOptionsQueryResult>
    {
        public long EventDetailId { get; set; }
        public Guid UserId { get; set; }
    }
}