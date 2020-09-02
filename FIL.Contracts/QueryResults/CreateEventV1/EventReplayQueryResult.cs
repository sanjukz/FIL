using FIL.Contracts.Models.CreateEventV1;
using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.CreateEventV1
{
    public class EventReplayQueryResult
    {
        public bool Success { get; set; }
        public string CompletedStep { get; set; }
        public short CurrentStep { get; set; }
        public long EventId { get; set; }
        public List<ReplayDetailModel> ReplayDetailModel { get; set; }
    }
}