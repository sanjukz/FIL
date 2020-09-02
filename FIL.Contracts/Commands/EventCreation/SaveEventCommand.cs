using FIL.Contracts.Enums;
using System;

namespace FIL.Contracts.Commands.EventCreation
{
    public class SaveEventCommand : Contracts.Interfaces.Commands.ICommandWithResult<SaveEventDataResult>
    {
        public long Id { get; set; }
        public Guid AltId { get; set; }
        public int EventCategoryId { get; set; }
        public EventType EventType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string MetaDetails { get; set; }
        public string TermsAndConditions { get; set; }
        public string TilesSliderImages { get; set; }
        public Guid TilesSliderImage { get; set; }
        public int ClientPointOfContactId { get; set; }
        public string TagIds { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsFeel { get; set; }
        public string SubCategoryIds { get; set; }
        public string AmenityIds { get; set; }
        public string TimeDuration { get; set; }
        public bool IsEdit { get; set; }
        public bool IsEventCreation { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class SaveEventDataResult : Contracts.Interfaces.Commands.ICommandResult
    {
        public long Id { get; set; }
        public Guid AltId { get; set; }
        public bool IsAlreadyExists { get; set; }
        public Guid ModifiedBy { get; set; }
    }
}