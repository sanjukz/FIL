using System;

namespace FIL.Contracts.Commands.Redemption
{
    public class BarcodeRedemptionCommand : Contracts.Interfaces.Commands.ICommandWithResult<BarcodeRedemptionCommandResult>
    {
        public string BarcodeNumber { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class BarcodeRedemptionCommandResult : Contracts.Interfaces.Commands.ICommandResult
    {
        public long Id { get; set; }
        public Guid AltId { get; set; }
        public string BarcodeNumber { get; set; }
        public bool IsConsumed { get; set; }
        public DateTime? ConsumedDateTime { get; set; }
        public bool Success { get; set; }
    }
}