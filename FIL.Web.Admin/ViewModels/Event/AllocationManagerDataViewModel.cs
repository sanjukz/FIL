﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Kitms.Feel.ViewModels.Event
{
    public class AllocationManagerDataViewModel
    {
        [Required]
        public List<AllocationManagerVenueDataViewModel> AllocationsData { get; set; }
    }
}