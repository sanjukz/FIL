using System.Collections.Generic;

namespace FIL.Contracts.Commands.BoxOffice
{
    public class RequestPartialVoidCommand : BaseCommand
    {
        public List<string> Barcodes { get; set; }
        public string Reason { get; set; }
    }
}