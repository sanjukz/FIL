﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Kitms.Feel.ViewModels.State
{
    public class StateFormDataViewModel
    {
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public Guid CountryAltId { get; set; }
    }
}