using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults;
using System;

namespace FIL.Contracts.Queries.TeamMatchAttribute
{
    public class TeamMatchAttributeQuery : IQuery<TeamMatchAttributeQueryResult>
    {
        public Guid EventAltId { get; set; }
    }
}