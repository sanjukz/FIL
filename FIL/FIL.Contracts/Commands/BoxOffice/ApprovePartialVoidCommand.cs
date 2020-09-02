using System.Collections.Generic;

namespace FIL.Contracts.Commands.BoxOffice
{
    public class ApprovePartialVoidCommand : BaseCommand
    {
        public List<string> Barcodes { get; set; }
    }
}