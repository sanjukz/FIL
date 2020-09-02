using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.Models.TMS;
using FIL.Contracts.QueryResults.TMS.Booking;
using System.Collections.Generic;

namespace FIL.Contracts.Queries.TMS.Booking
{
    public class CorporateOrderQuery : IQuery<CorporateOrderQueryResult>
    {
        public List<CorpoarteOrderDetail> corpoarteOrderDetails { get; set; }
        public List<SeatDetail> seatDetails { get; set; }
        public AllocationType AllocationTypeId { get; set; }
    }
}