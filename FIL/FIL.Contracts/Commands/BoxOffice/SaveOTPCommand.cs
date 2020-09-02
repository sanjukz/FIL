using System;

namespace FIL.Contracts.Commands.BoxOffice
{
    public class SaveOTPCommand : BaseCommand

    {
        public int PickupOTP { get; set; }
        public Guid? TransDetailAltId { get; set; }
        public long? TransactionDetailId { get; set; }
    }
}