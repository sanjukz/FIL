using FIL.Contracts.Models;
using System;
using System.Collections.Generic;

namespace FIL.Contracts.Commands.PlaceCalendar
{
    public class PlaceCalendarCommand : Contracts.Interfaces.Commands.ICommandWithResult<PlaceCalendarCommandResult>
    {
        public Guid PlaceAltId { get; set; }
        public FIL.Contracts.Enums.EventType PlaceType { get; set; }
        public List<Boolean> WeekOffDays { get; set; }
        public DateTime PlaceStartDate { get; set; }
        public DateTime PlaceEndDate { get; set; }
        public Guid VenueAltId { get; set; }
        public bool IsEdit { get; set; }
        public List<DateTime> HolidayDates { get; set; }
        public List<Timing> PlaceTimings { get; set; }
        public Guid ModifiedBy { get; set; }
        public RegularViewModel RegularTimeModel;
        public List<SeasonViewModel> SeasonTimeModel;
        public List<SpecialDayViewModel> SpecialDayModel;
        public bool IsNewCalendar;
        public bool IsEventCreation;
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
        public TimeViewModel[] Time;
    }

    public class SeasonViewModel
    {
        public int Id;
        public bool IsSameTime;
        public DateTime StartDate;
        public DateTime EndDate;
        public string Name;
        public TimeViewModel[] SameTime;
        public SpeecialDateSeasonTimeViewModel[] Time;
        public bool[] DaysOpen;
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
        public TimeViewModel[] Time;
    }

    public class RegularViewModel
    {
        public bool IsSameTime;
        public CustomTimeModelData[] CustomTimeModel;
        public TimeViewModel[] TimeModel;
        public bool[] DaysOpen;
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

    public class PlaceCalendarCommandResult : Contracts.Interfaces.Commands.ICommandResult
    {
        public long Id { get; set; }
        public List<EventDetail> eventDetails { get; set; }
        public bool Success { get; set; }
    }
}