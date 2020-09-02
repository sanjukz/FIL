using FIL.Contracts.Enums;
using System;

namespace FIL.Contracts.Commands.EventCreation
{
    public class EventLearnMoreAttributeCommand : BaseCommand
    {
        public int Id { get; set; }
        public Guid AltId { get; set; }
        public long EventId { get; set; }
        public LearnMoreFeatures LearnMoreFeatureId { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public bool IsEnabled { get; set; }
    }
}