﻿using FIL.Contracts.DataModels;
using FIL.Contracts.Models;
using System.Collections.Generic;

namespace FIL.Contracts.Queries.SeatLayout
{
    public class SeatLayoutQueryResult
    {
        public List<Row> Rows { get; set; }
        public List<MatchLayoutCompanionSeatMapping> MatchLayoutCompanionSeatMappings { get; set; }
    }
}