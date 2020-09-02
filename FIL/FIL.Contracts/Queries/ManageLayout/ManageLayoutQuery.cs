using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults;
using System;

namespace FIL.Contracts.Queries.ManageLayout
{
    public class ManageLayoutQuery : IQuery<ManageLayoutQueryResult>
    {
        public int Id { get; set; }
        public Guid AltId { get; set; }
    }
}