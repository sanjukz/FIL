using System.Collections.Generic;

namespace FIL.Contracts.Commands.FeelRedemption
{
    public class FeelRedemptionCommand : BaseCommand
    {
        public List<long> TransactionDetailIds { get; set; }
    }
}