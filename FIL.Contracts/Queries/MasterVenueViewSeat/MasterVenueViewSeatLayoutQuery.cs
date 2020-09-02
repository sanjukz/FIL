using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.MasterVenueViewSeat;

namespace FIL.Contracts.Queries.MasterVenueViewSeat
{
    public class MasterVenueViewSeatLayoutQuery : IQuery<MasterVenueViewSeatLayoutQueryResult>
    {
        public int Id { get; set; }
    }
}