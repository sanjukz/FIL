using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.CreateEventV1;

namespace FIL.Contracts.Queries.CreateEventV1
{
    public class StepQuery : IQuery<StepsQueryResult>
    {
        public long EventId { get; set; }
        public FIL.Contracts.Enums.MasterEventType MasterEventType { get; set; }
    }
}