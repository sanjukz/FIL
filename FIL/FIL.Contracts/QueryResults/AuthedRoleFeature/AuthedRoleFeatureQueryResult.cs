using FIL.Contracts.Models;
using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.AuthedRoleFeature
{
    public class AuthedRoleFeatureQueryResult
    {
        public IEnumerable<Feature> Feature { get; set; }
    }
}