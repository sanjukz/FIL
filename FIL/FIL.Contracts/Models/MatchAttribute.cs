using System;

namespace FIL.Contracts.Models
{
    public class MatchAttribute
    {
        public long EventDetailId { get; set; }
        public long Id { get; set; }
        public long TeamA { get; set; }
        public long TeamB { get; set; }
        public int MatchNo { get; set; }
        public int MatchDay { get; set; }
        public DateTime MatchStartTime { get; set; }
    }
}