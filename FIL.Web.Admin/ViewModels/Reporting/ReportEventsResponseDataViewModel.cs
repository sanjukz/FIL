﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Admin.ViewModels.Reporting
{
    public class ReportEventsResponseDataViewModel
    {
        public IEnumerable<FIL.Contracts.Models.Event> Events { get; set; }
    }
}
