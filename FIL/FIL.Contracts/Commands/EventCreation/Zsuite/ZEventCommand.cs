using FIL.Contracts.Enums;
using System;

namespace FIL.Contracts.Commands.EventCreation.Zsuite
{
    public class ZEventCommand : Contracts.Interfaces.Commands.ICommandWithResult<ZEventCommandResult>
    {
        public long? Id { get; set; }
        public int EventCategoryId { get; set; }
        public EventType EventType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string TermsAndConditions { get; set; }
        public string MetaDetails { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsEditEvent { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class ZEventCommandResult : Contracts.Interfaces.Commands.ICommandResult
    {
        public long Id { get; set; }
        public Guid AltId { get; set; }
        public bool IsAlreadyExists { get; set; }
    }
}