using System;
using System.Collections.Generic;

namespace FIL.Contracts.Models
{
    public class CalendarResult
    {
        public RegularViewModel RegularTimeModel;
        public List<SeasonViewModel> SeasonTimeModel;
        public List<SpecialDayViewModel> SpecialDayModel;
    }

    public class TimeViewModel
    {
        public int Id;
        public string From;
        public string To;
    }

    public class SpeecialDateSeasonTimeViewModel
    {
        public string Day;
        public int Id;
        public List<TimeViewModel> Time;
    }

    public class SeasonViewModel
    {
        public int Id;
        public bool IsSameTime;
        public DateTime StartDate;
        public DateTime EndDate;
        public string Name;
        public List<TimeViewModel> SameTime;
        public List<SpeecialDateSeasonTimeViewModel> Time;
        public List<bool> DaysOpen;
    }

    public class SpecailDatimeViewModel
    {
        public string Day;
        public TimeViewModel Time;
    }

    public class CustomTimeModelData
    {
        public string Day;
        public int Id;
        public List<TimeViewModel> Time;
    }

    public class RegularViewModel
    {
        public bool IsSameTime;
        public List<CustomTimeModelData> CustomTimeModel;
        public List<TimeViewModel> TimeModel;
        public List<bool> DaysOpen;
    }

    public class SpecialDayViewModel
    {
        public int Id;
        public string Name;
        public DateTime SpecialDate;
        public string From;
        public string To;
    }

    public class Timing
    {
        public string From;
        public string To;
    }
}