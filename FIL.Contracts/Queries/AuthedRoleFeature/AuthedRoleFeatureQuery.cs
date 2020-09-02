using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.AuthedRoleFeature;
using System;

namespace FIL.Contracts.Queries.AuthedRoleFeature
{
    public class AuthedRoleFeatureQuery : IQuery<AuthedRoleFeatureQueryResult>
    {
        public Guid UserAltId { get; set; }
    }
}