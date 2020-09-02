using System;

namespace FIL.Contracts.Commands.EventCreation
{
    public class SaveTicketFeeDetailCommand : Contracts.Interfaces.Commands.ICommandWithResult<SaveEventTicketFeeDataResult>
    {
        public long EventTicketAttributeId { get; set; }
        public short FeedId { get; set; }
        public string DisplayName { get; set; }
        public short ValueTypeId { get; set; }
        public decimal Value { get; set; }
        public short? FeeGroupId { get; set; }
        public bool IsEnabled { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class SaveEventTicketFeeDataResult : Contracts.Interfaces.Commands.ICommandResult
    {
        public long Id { get; set; }
        public Guid ModifiedBy { get; set; }
    }
}