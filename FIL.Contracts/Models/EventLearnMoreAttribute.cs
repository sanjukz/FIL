using FIL.Contracts.Enums;
using System;

namespace FIL.Contracts.Models
{
    public class EventLearnMoreAttribute
    {
        public Guid AltId { get; set; }
        public long EventId { get; set; }
        public LearnMoreFeatures LearnMoreFeatureId { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
    }
}