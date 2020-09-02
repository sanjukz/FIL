using System;

namespace FIL.Contracts.Commands.EventCreation
{
    public class SaveEventTicketDetailCommand : Contracts.Interfaces.Commands.ICommandWithResult<SaveEventTicketDetailResult>
    {
        public long Id { get; set; }
        public long EventDetailId { get; set; }
        public long TicketCategoryId { get; set; }
        public bool IsEnabled { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class SaveEventTicketDetailResult : Contracts.Interfaces.Commands.ICommandResult
    {
        public long Id { get; set; }
        public Guid ModifiedBy { get; set; }
    }
}