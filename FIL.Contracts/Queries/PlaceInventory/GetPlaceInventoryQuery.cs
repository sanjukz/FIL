using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.PlaceInventory;
using System;

namespace FIL.Contracts.Queries.PlaceInventory
{
    public class GetPlaceInventoryQuery : IQuery<GetPlaceInventoryQueryResult>
    {
        public Guid PlaceAltId { get; set; }
        public bool IsLiveOnline { get; set; }
    }
}