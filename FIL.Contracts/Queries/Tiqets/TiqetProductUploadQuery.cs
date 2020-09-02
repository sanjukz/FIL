using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.Tiqets;

namespace FIL.Contracts.Queries
{
    public class TiqetProductUploadQuery : IQuery<TiqetProductUploadQueryResult>
    {
        public int SkipIndex { get; set; }
        public int TakeIndex { get; set; }
    }
}