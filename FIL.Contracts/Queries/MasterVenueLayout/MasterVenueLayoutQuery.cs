using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.MasterVenueLayout;
using System;

namespace FIL.Contracts.Queries.MasterVenueLayout
{
    public class MasterVenueLayoutQuery : IQuery<MasterVenueLayoutQueryResult>
    {
        public Guid AltId { get; set; }
    }
}