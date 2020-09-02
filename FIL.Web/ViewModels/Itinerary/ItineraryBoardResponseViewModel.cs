using FIL.Contracts.DataModels;
using FIL.Web.Feel.ViewModels.Default;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Feel.ViewModels.Itinerary
{
    public class ItineraryBoardResponseViewModel
    {
        public List<List<FIL.Contracts.DataModels.SearchVenue>> ItineraryBoardData { get; set; }
        public SearchVenue CurrentPlace { get; set; }
        public bool Success { get; set; }
        public bool IsValidDandD { get; set; }
        public bool IsSourceCountZero { get; set; }
        public bool IsTargetDateExceed { get; set; }
        public bool IsDiffCityDandD { get; set; }
    }
}
