﻿using FIL.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Feel.ViewModels.EventCreation
{
    public class GetSavedEventsDaybyDayDataViewModel
    {
        public string QueryString { get; set; }
        public DateTime StartDate { get; set; }
        public List<int> CityIds { get; set; }
        public DateTime EndDate { get; set; }
        public TravelSpeed Speed { get; set; }
        public string Categories { get; set; }
        public BudgetRange BudgetRange { get; set; }
        public decimal NoOfAdults { get; set; }
    }
}
