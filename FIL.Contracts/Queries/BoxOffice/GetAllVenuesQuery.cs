using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.BoxOffice;
using System;

namespace FIL.Contracts.Queries.BoxOffice
{
    public class GetAllVenuesQuery : IQuery<GetAllVenueQueryResult>
    {
        public Guid EventAltID { get; set; }
    }
}