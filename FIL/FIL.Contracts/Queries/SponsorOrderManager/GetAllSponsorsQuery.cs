using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResult.SponsorOrderManager;
using System;

namespace FIL.Contracts.Queries.SponsorOrderManager
{
    public class GetAllSponsorsQuery : IQuery<GetAllSponsorsQueryResult>
    {
        public Guid UserAltId { get; set; }
    }
}