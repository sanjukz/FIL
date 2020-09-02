using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.Events;
using System;

namespace FIL.Contracts.Queries.BoxOffice
{
    public class GetAllEventDetailsQuery : IQuery<EventDetailQueryResult>
    {
        public Guid EventAltId { get; set; }
    }
}