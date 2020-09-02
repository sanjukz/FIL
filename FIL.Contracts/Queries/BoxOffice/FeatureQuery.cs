using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.BoxOffice;

namespace FIL.Contracts.Queries.BoxOffice
{
    public class FeatureQuery : IQuery<FeatureQueryResult>
    {
        public int RoleId { get; set; }
    }
}