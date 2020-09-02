using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults;
using System;

namespace FIL.Contracts.Queries.Creator
{
    public class MyFeelsQuery : IQuery<MyFeelsQueryResult>
    {
        public Guid CreatedBy { get; set; }
        public bool IsApproveModerateScreen { get; set; }
        public bool IsActive { get; set; }
    }
}