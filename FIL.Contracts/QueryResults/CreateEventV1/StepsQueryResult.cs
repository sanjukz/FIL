﻿using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.CreateEventV1
{
    public class StepsQueryResult
    {
        public long EventId { get; set; }
        public bool Success { get; set; }
        public bool IsTransacted { get; set; }
        public string CompletedStep { get; set; }
        public short CurrentStep { get; set; }
        public string EventName { get; set; }
        public FIL.Contracts.Enums.EventStatus EventStatus { get; set; }
        public FIL.Contracts.Enums.EventFrequencyType EventFrequencyType { get; set; }
        public List<FIL.Contracts.Models.CreateEventV1.StepModel> StepModel { get; set; }
    }
}