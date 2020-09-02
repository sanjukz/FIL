using System.Collections.Generic;

namespace FIL.Contracts.Commands.BoxOffice
{
    public class PartialVoidTransactionCommand : BaseCommand
    {
        public List<string> Barcodes { get; set; }
        public long? ExchangeTransactionId { get; set; }
        public long? ExchangedAmount { get; set; }
        public short? ActionTypeId { get; set; }
    }
}