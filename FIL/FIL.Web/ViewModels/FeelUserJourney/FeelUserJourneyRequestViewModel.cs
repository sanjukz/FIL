using FIL.Contracts.Enums;
using FIL.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Feel.ViewModels.FeelUserJourney
{
    public class FeelUserJourneyRequestViewModel
    {
        public PageType PageType { get; set; }
        public string PagePath { get; set; }
        public int Category { get; set; }
        public int SubCategory { get; set; }
        public int Country { get; set; }
        public int State { get; set; }
        public int City { get; set; }
        public FIL.Contracts.Enums.MasterEventType MasterEventType { get; set; }
    }
}
