﻿using FIL.Contracts.Models;
using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.BoxOffice
{
    public class FloatReportQueryResult
    {
        public List<FloatDetail> FloatDetails { get; set; }
        public List<FIL.Contracts.Models.User> Users { get; set; }
    }
}