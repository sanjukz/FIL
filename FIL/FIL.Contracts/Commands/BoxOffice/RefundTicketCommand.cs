using System;

namespace FIL.Contracts.Commands.BoxOffice
{
    public class RefundTicketCommand : BaseCommand
    {
        public string BarcodeNumber { get; set; }
        public Decimal RefundedAmount { get; set; }
    }
}