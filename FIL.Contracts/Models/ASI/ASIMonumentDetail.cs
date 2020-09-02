using System;
using System.Collections.Generic;

namespace FIL.Contracts.Models.ASI
{
    public class Amount
    {
        public string Type { get; set; }
        public int Total { get; set; }
        public int ASI { get; set; }
        public int LDA { get; set; }
        public int others { get; set; }
        public int MSM { get; set; }
        public int AC { get; set; }
    }

    public class Option
    {
        public string Type { get; set; }
        public string Text { get; set; }
    }

    public class Amount2
    {
        public string Type { get; set; }
        public int Total { get; set; }
        public int ASI { get; set; }
    }

    public class Optional
    {
        public string Name { get; set; }
        public List<Option> Options { get; set; }
        public List<Amount2> Amounts { get; set; }
    }

    public class Configuration
    {
        public List<Amount> Amounts { get; set; }
        public List<Optional> Optionals { get; set; }
    }

    public class Week
    {
        public bool Sunday { get; set; }
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
    }

    public class TimeSlot
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }

    public class Availability
    {
        public Week Week { get; set; }
        public List<TimeSlot> TimeSlots { get; set; }
        public List<DateTime> Holidays { get; set; }
        public DateTime MaxDate { get; set; }
    }

    public class MonumentDetailData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string AppConfigVersion { get; set; }
        public object Comment { get; set; }
        public string Version { get; set; }
        public string Circle { get; set; }
        public Configuration Configuration { get; set; }
        public Availability Availability { get; set; }
        public string Status { get; set; }
    }

    public class ASIMonumentDetail
    {
        public bool IsSuccess { get; set; }
        public MonumentDetailData Data { get; set; }
    }
}