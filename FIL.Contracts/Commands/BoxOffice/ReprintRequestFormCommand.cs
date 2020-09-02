using System.Collections.Generic;

namespace FIL.Contracts.Commands.BoxOffice
{
    public class ReprintRequestFormCommand : BaseCommand

    {
        public long TransactionId { get; set; }
        public string Reason { get; set; }
        public List<string> BarcodeId { get; set; }
    }
}