using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.FeelAdminPlaces;
using System;

namespace FIL.Contracts.Queries.FeelAdminPlaces
{
    public class FeelAdminPlacesQuery : IQuery<FeelAdminPlacesQueryResult>
    {
        public Guid UserAltId { get; set; }
        public bool IsMyFeel { get; set; }
        public bool IsFeelExists { get; set; }
        public bool IsDeactivateFeels { get; set; }
    }
}